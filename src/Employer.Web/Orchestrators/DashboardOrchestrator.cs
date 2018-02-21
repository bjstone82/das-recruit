﻿using Esfa.Recruit.Employer.Web.Services;
using Esfa.Recruit.Employer.Web.ViewModels;
using Esfa.Recruit.Storage.Client.Core.Messaging;
using Esfa.Recruit.Storage.Client.Core.Repositories;
using System.Threading.Tasks;
using System.Linq;

namespace Esfa.Recruit.Employer.Web.Orchestrators
{
    public class DashboardOrchestrator
    {
        private readonly IMessaging _messaging;
        private readonly IQueryVacancyRepository _queryRepository;
        private readonly IGetAssociatedEmployerAccountsService _getAccountService;

        public DashboardOrchestrator(IMessaging messaging, IGetAssociatedEmployerAccountsService getAccountsService, IQueryVacancyRepository queryRepository)
        {
            _messaging = messaging;
            _getAccountService = getAccountsService;
            _queryRepository = queryRepository;
        }

        public async Task<DashboardViewModel> GetDashboardViewModelAsync(string employerAccountId)
        {
            var account = await _getAccountService.GetEmployerAccountAsync(employerAccountId);
            var vacancies = await _queryRepository.GetVacanciesAsync(employerAccountId);
            var vm = new DashboardViewModel
            {
                EmployerName = account.DasAccountName,
                Vacancies = vacancies.OrderByDescending(v => v.AuditVacancyCreated).ToList()
            };

            return vm;
        }
    }
}