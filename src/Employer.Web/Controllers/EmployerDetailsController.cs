﻿using Microsoft.AspNetCore.Mvc;
using Esfa.Recruit.Employer.Web.ViewModels.EmployerDetails;
using Esfa.Recruit.Employer.Web.Configuration.Routes;
using Esfa.Recruit.Employer.Web.Orchestrators;
using System.Threading.Tasks;
using System;

namespace Esfa.Recruit.Employer.Web.Controllers
{
    [Route("accounts/{employerAccountId}/vacancies/{vacancyId}")]
    public class EmployerDetailsController : Controller
    {
        private readonly EmployerDetailsOrchestrator _orchestrator;

        public EmployerDetailsController(EmployerDetailsOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("employer-details", Name = RouteNames.EmployerDetails_Index_Get)]
        public async Task<IActionResult> Index(Guid vacancyId)
        {
            var vm = await _orchestrator.GetIndexViewModelAsync(vacancyId);
            return View(vm);
        }

        [HttpPost("employer-details", Name = RouteNames.EmployerDetails_Index_Post)]
        public IActionResult Index(IndexViewModel vm)
        {
            return RedirectToRoute(RouteNames.LocationAndPosition_Index_Get);
        }
    }
}