﻿using Esfa.Recruit.Employer.Web.ViewModels.Part1;

namespace Esfa.Recruit.Employer.Web.ViewModels.LegalEntityAgreement
{
    public class LegalEntityAgreementSoftStopViewModel
    {
        public bool HasLegalEntityAgreement { get; set; }
        public string LegalEntityName { get; set; }

        public PartOnePageInfoViewModel PageInfo { get; set; }
    }
}