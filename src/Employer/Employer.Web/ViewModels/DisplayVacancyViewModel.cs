﻿using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Esfa.Recruit.Employer.Web.ViewModels
{
    public abstract class DisplayVacancyViewModel
    {
        public string ApplicationInstructions { get; internal set; }
        public string ApplicationUrl { get; internal set; }
        public bool CanDelete { get; internal set; }
        public bool CanSubmit { get; internal set; }
        public string ContactName { get; internal set; }
        public string ContactEmail { get; internal set; }
        public string ContactTelephone { get; internal set; }
        public string ClosingDate { get; internal set; }
        public string EmployerDescription { get; internal set; }
        public string EmployerName { get; internal set; }
        public string EmployerWebsiteUrl { get; internal set; }
        public string ExpectedDuration { get; internal set; }
        public string HoursPerWeek { get; internal set; }
        public Address Location { get; internal set; }
        public IEnumerable<string> EmployerAddressElements { get; internal set; }
        public string MapUrl { get; internal set; }
        public string NumberOfPositions { get; internal set; }
        public string NumberOfPositionsCaption { get; internal set; }
        public string OutcomeDescription { get; internal set; }
        public string PossibleStartDate { get; internal set; }
        public string ProviderName { get; internal set; }
        public IEnumerable<string> Qualifications { get; internal set; }
        public string ShortDescription { get; internal set; }
        public IEnumerable<string> Skills { get; internal set; }
        public string ThingsToConsider { get; internal set; }
        public string Title { get; internal set; }
        public string TrainingDescription { get; internal set; }
        public string TrainingTitle { get; internal set; }
        public string TrainingType { get; internal set; }
        public string TrainingLevel { get; internal set; }
        public string VacancyDescription { get; internal set; }
        public string VacancyReferenceNumber { get; internal set; }
        public string WageInfo { get; internal set; }
        public string WageText { get; internal set; }
        public string WorkingWeekDescription { get; internal set; }

        public bool HasClosingDate => !string.IsNullOrWhiteSpace(ClosingDate);

        public bool HasShortDescription => !string.IsNullOrWhiteSpace(ShortDescription);

        public bool HasProviderName => !string.IsNullOrWhiteSpace(ProviderName);

        public bool HasTrainingTitle => !string.IsNullOrWhiteSpace(TrainingTitle);

        public bool HasWorkingWeekDescription => !string.IsNullOrWhiteSpace(WorkingWeekDescription);

        public bool HasHoursPerWeek => !string.IsNullOrWhiteSpace(HoursPerWeek);

        public bool HasWageText => !string.IsNullOrWhiteSpace(WageText);

        public bool HasWageInfo => !string.IsNullOrWhiteSpace(WageInfo);

        public bool HasExpectedDuration => !string.IsNullOrWhiteSpace(ExpectedDuration);

        public bool HasPossibleStartDate => !string.IsNullOrWhiteSpace(PossibleStartDate);

        public bool HasTrainingLevel => !string.IsNullOrWhiteSpace(TrainingLevel);

        public bool HasNumberOfPositionsCaption => !string.IsNullOrWhiteSpace(NumberOfPositionsCaption);
        public bool HasMapUrl => !string.IsNullOrEmpty(MapUrl);

        public bool HasVacancyDescription => !string.IsNullOrWhiteSpace(VacancyDescription);

        public bool HasTrainingDescription => !string.IsNullOrWhiteSpace(TrainingDescription);

        public bool HasOutcomeDescription => !string.IsNullOrWhiteSpace(OutcomeDescription);

        public bool HasSkills => Skills != null && Skills.Any();

        public bool HasQualifications => Qualifications != null && Qualifications.Any();

        public bool HasThingsToConsider => !string.IsNullOrWhiteSpace(ThingsToConsider);

        public bool HasEmployerName => !string.IsNullOrWhiteSpace(EmployerName);

        public bool HasEmployerDescription => !string.IsNullOrWhiteSpace(EmployerDescription);

        public bool HasEmployerWebsiteUrl => !string.IsNullOrWhiteSpace(EmployerWebsiteUrl);

        public bool HasContactName => !string.IsNullOrWhiteSpace(ContactName);

        public bool HasContactEmail => !string.IsNullOrWhiteSpace(ContactEmail);

        public bool HasContactTelephone => !string.IsNullOrWhiteSpace(ContactTelephone);

        public bool HasEmployerAddressElements => EmployerAddressElements != null && EmployerAddressElements.Any();

        public bool HasApplicationInstructions => !string.IsNullOrWhiteSpace(ApplicationInstructions);

        public bool HasApplicationUrl => !string.IsNullOrWhiteSpace(ApplicationUrl);
    }
}