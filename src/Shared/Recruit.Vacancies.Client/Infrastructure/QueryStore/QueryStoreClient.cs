﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Application.QueryStore;
using Esfa.Recruit.Vacancies.Client.Domain.Projections;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore
{
    public class QueryStoreClient : IQueryStoreReader, IQueryStoreWriter
    {
        private readonly IQueryStore _queryStore;

        public QueryStoreClient(IQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public Task<Dashboard> GetDashboardAsync(string employerAccountId)
        {
            var key = QueryViewType.Dashboard.GetIdValue(employerAccountId);

            return _queryStore.GetAsync<Dashboard>(key);
        }

        public Task UpdateDashboardAsync(string employerAccountId, IEnumerable<VacancySummary> vacancySummaries)
        {
            var dashboardItem = new Dashboard
            {
                Id = QueryViewType.Dashboard.GetIdValue(employerAccountId),
                Type = QueryViewType.Dashboard.TypeName,
                Vacancies = vacancySummaries,
                LastUpdated = DateTime.UtcNow
            };

            return _queryStore.UpsertAsync<Dashboard>(dashboardItem);
        }

        public Task UpdateApprenticeshipProgrammesAsync(IEnumerable<ApprenticeshipProgramme> programmes)
        {
            var programmesItem = new ApprenticeshipProgrammes
            {
                Id = QueryViewType.ApprenticeshipProgrammes.GetIdValue(),
                Type = QueryViewType.ApprenticeshipProgrammes.TypeName,
                Programmes = programmes,
                LastUpdated = DateTime.UtcNow
            };
            
            return _queryStore.UpsertAsync<ApprenticeshipProgrammes>(programmesItem);
        }
    }
}