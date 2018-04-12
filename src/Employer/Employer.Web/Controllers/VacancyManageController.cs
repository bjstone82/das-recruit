﻿using System;
using System.Threading.Tasks;
using Esfa.Recruit.Employer.Web.Configuration.Routing;
using Esfa.Recruit.Employer.Web.Orchestrators;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Domain.Enums;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Client;
using Microsoft.AspNetCore.Mvc;

namespace Employer.Web.Controllers
{
    [Route(RoutePrefixPaths.AccountVacancyRoutePath)]
    public class VacancyManageController : Controller
    {
        private readonly VacancyManageOrchestrator _orchestrator;
        private readonly IVacancyClient _client;

        public VacancyManageController(VacancyManageOrchestrator orchestrator, IVacancyClient client)
        {
            _orchestrator = orchestrator;
            _client = client;
        }

        [HttpGet("", Name = RouteNames.DisplayVacancy_Get)]
        public async Task<IActionResult> DisplayVacancy([FromRoute]Guid vacancyId)
        {
            var vacancy = await _client.GetVacancyAsync(vacancyId);

            if (vacancy.Status == VacancyStatus.Draft)
            {
                return HandleRedirectOfDraftVacancy(vacancy);
            }

            var m = _orchestrator.GetVacancyDisplayViewModelAsync(vacancy);
            return View(m.ViewName, m.ViewModel);
        }

        private IActionResult HandleRedirectOfDraftVacancy(Vacancy vacancy)
        {
            if (!vacancy.Wage.WageType.HasValue)
            {
                return RedirectToRoute(RouteNames.Title_Get);
            }

            return RedirectToRoute(RouteNames.Vacancy_Preview_Get);
        }
    }
}