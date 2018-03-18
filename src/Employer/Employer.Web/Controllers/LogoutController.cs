﻿using Esfa.Recruit.Employer.Web.Configuration;
using Esfa.Recruit.Employer.Web.Configuration.Routing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Esfa.Recruit.Employer.Web.Controllers
{
    [Route(RoutePrefixPaths.AccountRoutePath)]
    public class LogoutController : Controller
    {
        private readonly ExternalLinksConfiguration _externalLinks;
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(IOptions<ExternalLinksConfiguration> externalLinksOptions, ILogger<LogoutController> logger)
        {
            _externalLinks = externalLinksOptions.Value;
            _logger = logger;
        }

        [HttpGet, Route("logout", Name = RouteNames.Logout_Get)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");

            return Redirect(_externalLinks.ManageApprenticeshipSiteUrl);
        }
    }
}