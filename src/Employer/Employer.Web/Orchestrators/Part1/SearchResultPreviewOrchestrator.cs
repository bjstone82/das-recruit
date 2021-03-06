using System.Threading.Tasks;
using Esfa.Recruit.Employer.Web.Configuration.Routing;
using Esfa.Recruit.Employer.Web.RouteModel;
using Esfa.Recruit.Employer.Web.ViewModels.Part1.SearchResultPreview;
using Esfa.Recruit.Shared.Web.Extensions;
using Esfa.Recruit.Shared.Web.Services;
using Esfa.Recruit.Vacancies.Client.Domain.Extensions;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Client;
using Esfa.Recruit.Vacancies.Client.Application.Providers;

namespace Esfa.Recruit.Employer.Web.Orchestrators.Part1
{
    public class SearchResultPreviewOrchestrator
    {
        private const int MapImageWidth = 190;
        private const int MapImageHeight = 125;
        private readonly IEmployerVacancyClient _client;
        private readonly IRecruitVacancyClient _vacancyClient;
        private readonly IGeocodeImageService _mapService;
        private readonly IMinimumWageProvider _wageProvider;

        public SearchResultPreviewOrchestrator(IEmployerVacancyClient client, IRecruitVacancyClient vacancyClient, IGeocodeImageService mapService, IMinimumWageProvider wageProvider)
        {
            _client = client;
            _vacancyClient = vacancyClient;
            _mapService = mapService;
            _wageProvider = wageProvider;
        }
        
        public async Task<SearchResultPreviewViewModel> GetSearchResultPreviewViewModelAsync(VacancyRouteModel vrm)
        {
            var vacancy = await Utility.GetAuthorisedVacancyForEditAsync(_client, _vacancyClient, vrm, RouteNames.SearchResultPreview_Get);

            var wagePeriod = _wageProvider.GetWagePeriod(vacancy.StartDate.Value);

            var vm = new SearchResultPreviewViewModel
            {
                EmployerName = vacancy.EmployerName,
                NumberOfPositions = vacancy.NumberOfPositions?.ToString(),
                ShortDescription = vacancy.ShortDescription,
                ClosingDate = vacancy.ClosingDate?.AsGdsDate(),
                StartDate = vacancy.StartDate?.AsGdsDate(),
                LevelName = await GetLevelName(vacancy.ProgrammeId),
                Title = vacancy.Title,
                Wage = vacancy.Wage?.ToText(vacancy.StartDate),
                HasYearlyWage = (vacancy.Wage != null && vacancy.Wage.WageType != WageType.Unspecified),
                IsDisabilityConfident = vacancy.IsDisabilityConfident
            };

            if (vacancy.EmployerLocation != null)
            {
                vm.MapUrl = vacancy.EmployerLocation.HasGeocode
                    ? _mapService.GetMapImageUrl(vacancy.EmployerLocation.Latitude.ToString(), vacancy.EmployerLocation.Longitude.ToString(), MapImageWidth, MapImageHeight)
                    : _mapService.GetMapImageUrl(vacancy.EmployerLocation?.Postcode, MapImageWidth, MapImageHeight);
            }
            
            return vm;
        }

        private async Task<string> GetLevelName(string programmeId)
        {
            if (string.IsNullOrWhiteSpace(programmeId))
                return null;

            var match = await _client.GetApprenticeshipProgrammeAsync(programmeId);

            return match?.Level.GetDisplayName();
        }
    }
}
