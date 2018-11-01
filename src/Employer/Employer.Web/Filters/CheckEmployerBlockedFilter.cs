﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Recruit.Employer.Web.Caching;
using Esfa.Recruit.Employer.Web.Configuration;
using Esfa.Recruit.Employer.Web.Configuration.Routing;
using Esfa.Recruit.Employer.Web.Controllers;
using Esfa.Recruit.Employer.Web.Exceptions;
using Esfa.Recruit.Vacancies.Client.Application.Providers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Esfa.Recruit.Employer.Web.Filters
{
    public class CheckEmployerBlockedFilter : IAsyncActionFilter
    {
        private readonly IBlockedEmployersProvider _blockedEmployersProvider;
        private readonly ICache _cache;

        private DateTime CacheAbsoluteExpiryTime => DateTime.UtcNow.AddHours(1);

        public CheckEmployerBlockedFilter(IBlockedEmployersProvider blockedEmployersProvider, ICache cache)
        {
            _blockedEmployersProvider = blockedEmployersProvider;
            _cache = cache;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (RequestIsForWhiteListedPage(context) == false)
            {
                var blockedEmployerAccountIds = await _cache.CacheAsideAsync(
                    CacheKeys.BlockedEmployersCacheKey,
                    CacheAbsoluteExpiryTime,
                    () => _blockedEmployersProvider.GetBlockedEmployerAccountIdsAsync());

                var accountIdFromUrl = context.RouteData.Values[RouteValues.EmployerAccountId].ToString().ToUpper();

                if (blockedEmployerAccountIds.Contains(accountIdFromUrl))
                {
                    throw new BlockedEmployerException($"Employer account '{accountIdFromUrl}' is blocked");
                }
            }

            await next();
        }

        private bool RequestIsForWhiteListedPage(ActionExecutingContext context)
        {
            var controllerName = (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerTypeInfo).Name;

            var whitelistControllers = new List<string>{ nameof(ErrorController), nameof(LogoutController), nameof(ExternalLinksController) };
            
            return whitelistControllers.Contains(controllerName);
        }
    }
}