using System.Threading.Tasks;
using Esfa.Recruit.Employer.Web.Configuration.Routing;
using Esfa.Recruit.Employer.Web.Mappings;
using Esfa.Recruit.Employer.Web.Mappings.Extensions;
using Esfa.Recruit.Employer.Web.RouteModel;
using Esfa.Recruit.Employer.Web.ViewModels.Part1.Training;
using Esfa.Recruit.Shared.Web.Extensions;
using Esfa.Recruit.Shared.Web.Orchestrators;
using Esfa.Recruit.Shared.Web.Services;
using Esfa.Recruit.Vacancies.Client.Application.Providers;
using Esfa.Recruit.Vacancies.Client.Application.Validation;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Client;
using Microsoft.Extensions.Logging;

namespace Esfa.Recruit.Employer.Web.Orchestrators.Part1
{
    public class TrainingOrchestrator : EntityValidatingOrchestrator<Vacancy, TrainingEditModel>
    {
        private const VacancyRuleSet ValdationRules = VacancyRuleSet.ClosingDate | VacancyRuleSet.StartDate | VacancyRuleSet.TrainingProgramme | VacancyRuleSet.StartDateEndDate | VacancyRuleSet.TrainingExpiryDate;
        private readonly IEmployerVacancyClient _client;
        private readonly IRecruitVacancyClient _vacancyClient;
        private readonly ITimeProvider _timeProvider;
        private readonly IReviewSummaryService _reviewSummaryService;

        public TrainingOrchestrator(IEmployerVacancyClient client, IRecruitVacancyClient vacancyClient, ILogger<TrainingOrchestrator> logger, ITimeProvider timeProvider, IReviewSummaryService reviewSummaryService) : base(logger)
        {
            _client = client;
            _vacancyClient = vacancyClient;
            _timeProvider = timeProvider;
            _reviewSummaryService = reviewSummaryService;
        }
        
        public async Task<TrainingViewModel> GetTrainingViewModelAsync(VacancyRouteModel vrm)
        {
            var vacancyTask = Utility.GetAuthorisedVacancyForEditAsync(_client, _vacancyClient, vrm, RouteNames.Training_Get);
            var programmesTask = _client.GetActiveApprenticeshipProgrammesAsync();

            await Task.WhenAll(vacancyTask, programmesTask);

            var vacancy = vacancyTask.Result;
            var programmes = programmesTask.Result;
            
            var vm = new TrainingViewModel
            {
                VacancyId = vacancy.Id,
                SelectedProgrammeId = vacancy.ProgrammeId,
                Programmes = programmes.ToViewModel(),
                IsDisabilityConfident = vacancy.IsDisabilityConfident,
                PageInfo = Utility.GetPartOnePageInfo(vacancy),
                CurrentYear = _timeProvider.Now.Year
            };

            if (vacancy.ClosingDate.HasValue)
            {
                vm.ClosingDay = $"{vacancy.ClosingDate.Value.Day:00}";
                vm.ClosingMonth = $"{vacancy.ClosingDate.Value.Month:00}";
                vm.ClosingYear = $"{vacancy.ClosingDate.Value.Year}";
            }

            if (vacancy.StartDate.HasValue)
            {
                vm.StartDay = $"{vacancy.StartDate.Value.Day:00}";
                vm.StartMonth = $"{vacancy.StartDate.Value.Month:00}";
                vm.StartYear = $"{vacancy.StartDate.Value.Year}";
            }

            if (vacancy.Status == VacancyStatus.Referred)
            {
                vm.Review = await _reviewSummaryService.GetReviewSummaryViewModelAsync(vacancy.VacancyReference.Value,
                    ReviewFieldMappingLookups.GetTrainingReviewFieldIndicators());
            }

            return vm;
        }

        public async Task<TrainingViewModel> GetTrainingViewModelAsync(TrainingEditModel m)
        {
            var vm = await GetTrainingViewModelAsync((VacancyRouteModel)m);

            vm.ClosingDay = m.ClosingDay;
            vm.ClosingMonth = m.ClosingMonth;
            vm.ClosingYear = m.ClosingYear;

            vm.StartDay = m.StartDay;
            vm.StartMonth = m.StartMonth;
            vm.StartYear = m.StartYear;

            vm.SelectedProgrammeId = m.SelectedProgrammeId;

            vm.IsDisabilityConfident = m.IsDisabilityConfident;

            return vm;
        }

        public async Task<OrchestratorResponse> PostTrainingEditModelAsync(TrainingEditModel m, VacancyUser user)
        {
            var vacancy = await Utility.GetAuthorisedVacancyForEditAsync(_client, _vacancyClient, m, RouteNames.Training_Post);

            vacancy.ClosingDate = m.ClosingDate.AsDateTimeUk()?.ToUniversalTime();
            vacancy.StartDate = m.StartDate.AsDateTimeUk()?.ToUniversalTime();
            vacancy.ProgrammeId = m.SelectedProgrammeId;
            vacancy.DisabilityConfident = m.IsDisabilityConfident ? DisabilityConfident.Yes : DisabilityConfident.No;
            
            return await ValidateAndExecute(
                vacancy, 
                v => _vacancyClient.Validate(v, ValdationRules),
                v => _vacancyClient.UpdateDraftVacancyAsync(vacancy, user)
            );
        }

        protected override EntityToViewModelPropertyMappings<Vacancy, TrainingEditModel> DefineMappings()
        {
            var mappings = new EntityToViewModelPropertyMappings<Vacancy, TrainingEditModel>();

            mappings.Add(e => e.ProgrammeId, vm => vm.SelectedProgrammeId);
            mappings.Add(e => e.StartDate, vm => vm.StartDate);
            mappings.Add(e => e.ClosingDate, vm => vm.ClosingDate);

            return mappings;
        }
    }
}
