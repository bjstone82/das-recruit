using System;
using Microsoft.AspNetCore.Mvc;

namespace Esfa.Recruit.Provider.Web.ViewModels.Part1.Employer
{
    public class EmployersEditViewModel
    {
        public string SelectedEmployerId { get; set; }

        [FromRoute]
        public string Ukprn { get; set; }
        [FromRoute]
        public Guid? VacancyId { get; set; }
    }
}