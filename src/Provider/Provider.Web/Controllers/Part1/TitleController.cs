using System.Threading.Tasks;
using Esfa.Recruit.Provider.Web.Configuration.Routing;
using Esfa.Recruit.Provider.Web.Extensions;
using Esfa.Recruit.Provider.Web.Orchestrators;
using Esfa.Recruit.Provider.Web.RouteModel;
using Esfa.Recruit.Provider.Web.ViewModels.Part1.Title;
using Microsoft.AspNetCore.Mvc;
using Esfa.Recruit.Shared.Web.Extensions;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Client;
using System;

namespace Esfa.Recruit.Provider.Web.Controllers.Part1
{
    [Route(RoutePaths.AccountRoutePath)]
    public class TitleController : Controller
    {
        private const string NewVacancyTitleRoute = "create-vacancy";
        private const string ExistingVacancyTitleRoute = "vacancies/{vacancyId:guid}/title";
        private readonly TitleOrchestrator _orchestrator;
        public IProviderVacancyClient ProviderVacancyClient { get; }

        public TitleController(TitleOrchestrator orchestrator, IProviderVacancyClient providerVacancyClient)
        {
            this.ProviderVacancyClient = providerVacancyClient;
            _orchestrator = orchestrator;
        }

        [HttpGet(NewVacancyTitleRoute, Name = RouteNames.CreateVacancy_Get)]
        public IActionResult Title([FromQuery] string employerAccountId)
        {
            var vm = _orchestrator.GetTitleViewModelForNewVacancy(employerAccountId, User.GetUkprn());
            vm.PageInfo.SetWizard();
            return View(vm);
        }


        // [HttpGet(ExistingVacancyTitleRoute, Name = RouteNames.Title_Get)]
        // public async Task<IActionResult> Title(VacancyRouteModel vrm, [FromQuery] string wizard = "true")
        // {            
        //     var vm = await _orchestrator.GetTitleViewModelAsync(vrm);
        //     vm.PageInfo.SetWizard(wizard);
        //     return View(vm);
        // }

        [HttpPost(NewVacancyTitleRoute, Name = RouteNames.CreateVacancy_Post)]
        //[HttpGet(ExistingVacancyTitleRoute, Name = RouteNames.Title_Post)]
        public async Task<IActionResult> Title(TitleEditModel model, [FromQuery] string employerAccountId, [FromQuery] bool wizard)
        {
            var ukprn = User.GetUkprn();
            var response = await _orchestrator.PostTitleEditModelAsync(model, User.ToVacancyUser(), ukprn);
            
            if (!response.Success)
            {
                response.AddErrorsToModelState(ModelState);
            }

            if(!ModelState.IsValid)
            {
                var vm = await _orchestrator.GetTitleViewModelAsync(model, ukprn);
                vm.PageInfo.SetWizard(wizard);
                return View(vm);
            }
            return  RedirectToRoute(RouteNames.ShortDescription_Get, new {vacancyId = response.Data});
            // return wizard
            //     ? RedirectToRoute(RouteNames.ShortDescription_Get, new {vacancyId = response.Data})
            //     : RedirectToRoute(RouteNames.Vacancy_Preview_Get);
        }    
    }
}