using System;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections
{
    public abstract class QueryProjectionBase
    {
        protected QueryProjectionBase(string viewType)
        {
            ViewType = viewType;
        }

        public string Id { get; set; }
        public string ViewType { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}