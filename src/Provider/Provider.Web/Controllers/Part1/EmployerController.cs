using System.Threading.Tasks;
using Esfa.Recruit.Provider.Web.Configuration.Routing;
using Esfa.Recruit.Provider.Web.RouteModel;
using Microsoft.AspNetCore.Mvc;
using Provider.Web.Orchestrators.Part1;

namespace Provider.Web.Controllers.Part1
{
    [Route(RoutePaths.AccountRoutePath)]
    public class EmployerController : Controller
    {
        private readonly EmployerOrchestrator _orchestrator;

        public EmployerController(EmployerOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("create-vacancy", Name = RouteNames.CreateVacancy_Get)]
        public async Task<IActionResult> Employer(VacancyRouteModel vacancyRouteModel, [FromQuery] string wizard = "true")
        {
            var vm = await _orchestrator.GetEmployersViewModelAsync(vacancyRouteModel);
            vm.PageInfo.SetWizard(wizard);
            return View(vm);
        }
    }
}