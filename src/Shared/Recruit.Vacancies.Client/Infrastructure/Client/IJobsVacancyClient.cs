using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Models;
using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.LiveVacancy;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Services.Models;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Client
{
    public interface IJobsVacancyClient
    {
        Task AssignVacancyNumber(Guid vacancyId);
        Task UpdateApprenticeshipProgrammesAsync(IEnumerable<ApprenticeshipProgramme> programmes);
        Task UpdateEmployerVacancyDataAsync(string employerAccountId, IEnumerable<LegalEntity> legalEntities);
        Task<IEnumerable<LegalEntity>> GetEmployerLegalEntitiesAsync(string employerAccountId);
        Task CreateVacancyReview(long vacancyReference);
        Task<IEnumerable<LiveVacancy>> GetLiveVacancies();
        Task CloseVacancy(Guid vacancyId);
        Task EnsureVacancyIsGeocodedAsync(Guid vacancyId);
        Task ApproveVacancy(long vacancyReference);
        Task ReferVacancy(long vacancyReference);
        Task CreateApplicationReviewAsync(Domain.Entities.Application application);
        Task<IEnumerable<TrainingProgrammeCategory>> GetTrainingProgrammeCategories();
        Task<IEnumerable<IApprenticeshipProgramme>> GetActiveApprenticeshipProgrammesAsync();
    }
}