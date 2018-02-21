﻿using Esfa.Recruit.Employer.Web.ViewModels.Sections;
using Recruit.Vacancies.Client.Infrastructure.Client;
using System;
using System.Threading.Tasks;

namespace Esfa.Recruit.Employer.Web.Orchestrators
{
    public class SectionsOrchestrator
    {
        private readonly IVacancyClient _client;

        public SectionsOrchestrator(IVacancyClient client)
        {
            _client = client;
        }

        public async Task<IndexViewModel> GetIndexViewModelAsync(Guid vacancyId)
        {
            var vacancy = await _client.GetVacancyForEditAsync(vacancyId);

            var vm = new IndexViewModel
            {
                Title = vacancy.Title
            };

            return vm;
        }
    }
}