﻿using Esfa.Recruit.Vacancies.Client.Application.Providers;
using Esfa.Recruit.Vacancies.Client.Application.Services;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Domain.Repositories;
using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.ApplicationReview;
using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Employer;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Services.Projections
{
    internal class EmployerDashboardProjectionService : IEmployerDashboardProjectionService
    {
        private readonly ILogger<EmployerDashboardProjectionService> _logger;
        private readonly IVacancyRepository _repository;
        private readonly IQueryStoreWriter _queryStoreWriter;
        private readonly IApplicationReviewRepository _applicationReviewRepository;
        private readonly IApprenticeshipProgrammeProvider _apprenticeshipProgrammeProvider;
        private readonly ITimeProvider _timeProvider;

        public EmployerDashboardProjectionService(
            IVacancyRepository repository, 
            IApplicationReviewRepository applicationReviewRepository, 
            IQueryStoreWriter queryStoreWriter, 
            ILogger<EmployerDashboardProjectionService> logger,
            IApprenticeshipProgrammeProvider apprenticeshipProgrammeProvider,
            ITimeProvider timeProvider)
        {
            _logger = logger;
            _repository = repository;
            _queryStoreWriter = queryStoreWriter;
            _applicationReviewRepository = applicationReviewRepository;
            _apprenticeshipProgrammeProvider = apprenticeshipProgrammeProvider;
            _timeProvider = timeProvider;
        }

        public async Task ReBuildAllDashboardsAsync()
        {
            var employerAccountIds = (await _repository.GetDistinctEmployerAccounts()).ToList();

            _logger.LogInformation($"Rebuilding {employerAccountIds.Count} dashboards");

            var startDateTime = _timeProvider.Now;
            var stopwatch = Stopwatch.StartNew();

            foreach (var id in employerAccountIds)
            {
                await ReBuildDashboardAsync(id);
            }

            _logger.LogInformation($"Rebuilding {employerAccountIds.Count} dashboards took {stopwatch.ElapsedMilliseconds} milliseconds");

            var count = await _queryStoreWriter.RemoveOldEmployerDashboards(startDateTime);

            _logger.LogInformation($"Removed {count} redundant/old employer dashboards from query store");
        }

        public async Task ReBuildDashboardAsync(string employerAccountId)
        {
            var vacancySummariesTask = _repository.GetVacanciesByEmployerAccountAsync<VacancySummary>(employerAccountId);

            var applicationReviewsTask = _applicationReviewRepository.GetForEmployerAsync<ApplicationReviewSummary>(
                    employerAccountId);

            await Task.WhenAll(vacancySummariesTask, applicationReviewsTask);

            var vacancySummaries = vacancySummariesTask.Result;
            var applicationReviews = applicationReviewsTask.Result;

            var activeVacancySummaries = vacancySummaries.Where(v => v.IsDeleted == false).ToList();

            foreach (var summary in activeVacancySummaries)
            {
                if (summary.VacancyReference.HasValue)
                {
                    var vacancyApplicationReviews = applicationReviews.Where(r => r.VacancyReference == summary.VacancyReference.Value).ToList();
                    summary.AllApplicationsCount = vacancyApplicationReviews.Count;
                    summary.NewApplicationsCount = vacancyApplicationReviews.Count(r => r.Status == ApplicationReviewStatus.New);
                }

                await UpdateWithTrainingProgrammeInfo(summary);
            }

            await _queryStoreWriter.UpdateEmployerDashboardAsync(employerAccountId, activeVacancySummaries.OrderBy(v => v.CreatedDate));

            _logger.LogDebug("Update dashboard with {count} summary records for account: {employerAccountId}", activeVacancySummaries.Count, employerAccountId);
        }

        private async Task UpdateWithTrainingProgrammeInfo(VacancySummary summary)
        {
            if (summary.ProgrammeId != null)
            {
                var programme = await _apprenticeshipProgrammeProvider.GetApprenticeshipProgrammeAsync(summary.ProgrammeId);

                if (programme == null)
                {
                    _logger.LogWarning($"No training programme found for ProgrammeId: {summary.ProgrammeId}");
                }
                else
                {
                    summary.TrainingTitle = programme.Title;
                    summary.TrainingType = programme.ApprenticeshipType;
                    summary.TrainingLevel = programme.Level;
                }
            }
        }
    }
}
