using System;
using System.Collections.Generic;
using Esfa.Recruit.Vacancies.Client.Infrastructure.ReferenceData;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.ReferenceData.BlockedEmployers
{
    public class BlockedEmployers : IReferenceDataItem
    {
        public string Id { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public List<string> EmployerAccountIds { get; set; }
    }
}