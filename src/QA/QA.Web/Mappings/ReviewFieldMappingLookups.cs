using System.Collections.Generic;
using Esfa.Recruit.Shared.Web.Mappers;
using Esfa.Recruit.Shared.Web.ViewModels;
using Esfa.Recruit.Vacancies.Client.Application.Services;

namespace Esfa.Recruit.Qa.Web.Mappings
{
    public static class ReviewFieldMappingLookups
    {
        public static ReviewFieldMappingLookupsForPage GetPreviewReviewFieldIndicators()
        {
            var vms = new List<ReviewFieldIndicatorViewModel>
            {
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.Title, Anchors.Title),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.ShortDescription, Anchors.ShortDescription),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.ClosingDate, Anchors.ClosingDate),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.WorkingWeek, Anchors.WorkingWeek),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.Wage, Anchors.YearlyWage),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.ExpectedDuration, Anchors.ExpectedDuration),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.PossibleStartDate, Anchors.PossibleStartDate),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.TrainingLevel, Anchors.TrainingLevel),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.NumberOfPositions, Anchors.NumberOfPositions),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.VacancyDescription, Anchors.VacancyDescription),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.TrainingDescription, Anchors.TrainingDescription),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.OutcomeDescription, Anchors.OutcomeDescription),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.Skills, Anchors.Skills),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.Qualifications, Anchors.Qualifications),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.ThingsToConsider, Anchors.ThingsToConsider),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.EmployerDescription, Anchors.EmployerDescription),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.DisabilityConfident, Anchors.DisabilityConfident),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.EmployerWebsiteUrl, Anchors.EmployerWebsiteUrl),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.EmployerContact, Anchors.EmployerContact),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.EmployerAddress, Anchors.EmployerAddress),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.Provider, Anchors.Provider),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.Training, Anchors.Training),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.ApplicationMethod, Anchors.ApplicationMethod),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.ApplicationUrl, Anchors.ApplicationUrl),
                new ReviewFieldIndicatorViewModel(FieldIdentifiers.ApplicationInstructions, Anchors.ApplicationInstructions)
            };

            var mappings =  new Dictionary<string, IEnumerable<string>>
            {
                { FieldIdResolver.ToFieldId(v => v.EmployerAccountId), new string[0]},
                { FieldIdResolver.ToFieldId(v => v.Title), new[]{ FieldIdentifiers.Title} },
                { FieldIdResolver.ToFieldId(v => v.EmployerName), new string[0] },
                { FieldIdResolver.ToFieldId(v => v.ShortDescription), new []{ FieldIdentifiers.ShortDescription} },
                { FieldIdResolver.ToFieldId(v => v.ClosingDate), new []{ FieldIdentifiers.ClosingDate} },
                { FieldIdResolver.ToFieldId(v => v.Wage.WeeklyHours), new []{ FieldIdentifiers.WorkingWeek} },
                { FieldIdResolver.ToFieldId(v => v.Wage.WorkingWeekDescription), new []{ FieldIdentifiers.WorkingWeek} },
                { FieldIdResolver.ToFieldId(v => v.Wage.WageAdditionalInformation), new []{ FieldIdentifiers.Wage}},
                { FieldIdResolver.ToFieldId(v => v.Wage.WageType),  new[]{ FieldIdentifiers.Wage}},
                { FieldIdResolver.ToFieldId(v => v.Wage.FixedWageYearlyAmount), new []{ FieldIdentifiers.Wage }},
                { FieldIdResolver.ToFieldId(v => v.Wage.Duration), new []{ FieldIdentifiers.ExpectedDuration }},
                { FieldIdResolver.ToFieldId(v => v.Wage.DurationUnit), new []{ FieldIdentifiers.ExpectedDuration }},
                { FieldIdResolver.ToFieldId(v => v.StartDate), new []{ FieldIdentifiers.PossibleStartDate} },
                { FieldIdResolver.ToFieldId(v => v.ProgrammeId), new []{ FieldIdentifiers.TrainingLevel, FieldIdentifiers.Training} },
                { FieldIdResolver.ToFieldId(v => v.VacancyReference), new string[0] },
                { FieldIdResolver.ToFieldId(v => v.NumberOfPositions), new []{ FieldIdentifiers.NumberOfPositions} },
                { FieldIdResolver.ToFieldId(v => v.Description), new []{ FieldIdentifiers.VacancyDescription} },
                { FieldIdResolver.ToFieldId(v => v.TrainingDescription), new []{ FieldIdentifiers.TrainingDescription} },
                { FieldIdResolver.ToFieldId(v => v.OutcomeDescription), new []{ FieldIdentifiers.OutcomeDescription} },
                { FieldIdResolver.ToFieldId(v => v.Skills), new []{ FieldIdentifiers.Skills} },
                { FieldIdResolver.ToFieldId(v => v.Qualifications), new []{ FieldIdentifiers.Qualifications} },
                { FieldIdResolver.ToFieldId(v => v.ThingsToConsider), new []{ FieldIdentifiers.ThingsToConsider} },
                { FieldIdResolver.ToFieldId(v => v.EmployerDescription), new [] { FieldIdentifiers.EmployerDescription }},
                { FieldIdResolver.ToFieldId(v => v.DisabilityConfident), new []{ FieldIdentifiers.DisabilityConfident} },
                { FieldIdResolver.ToFieldId(v => v.EmployerWebsiteUrl), new []{ FieldIdentifiers.EmployerWebsiteUrl} },
                { FieldIdResolver.ToFieldId(v => v.EmployerLocation.AddressLine1), new []{ FieldIdentifiers.EmployerAddress }},
                { FieldIdResolver.ToFieldId(v => v.EmployerLocation.AddressLine2), new []{ FieldIdentifiers.EmployerAddress }},
                { FieldIdResolver.ToFieldId(v => v.EmployerLocation.AddressLine3), new []{ FieldIdentifiers.EmployerAddress }},
                { FieldIdResolver.ToFieldId(v => v.EmployerLocation.AddressLine4), new []{ FieldIdentifiers.EmployerAddress }},
                { FieldIdResolver.ToFieldId(v => v.EmployerLocation.Postcode), new[] { FieldIdentifiers.EmployerAddress}},
                { FieldIdResolver.ToFieldId(v => v.EmployerContact.Email), new []{ FieldIdentifiers.EmployerContact} },
                { FieldIdResolver.ToFieldId(v => v.EmployerContact.Name), new [] { FieldIdentifiers.EmployerContact }},
                { FieldIdResolver.ToFieldId(v => v.EmployerContact.Phone), new []{ FieldIdentifiers.EmployerContact }},
                { FieldIdResolver.ToFieldId(v => v.TrainingProvider.Ukprn) , new []{ FieldIdentifiers.Provider} },
                { FieldIdResolver.ToFieldId(v => v.ApplicationInstructions), new [] { FieldIdentifiers.ApplicationInstructions }},
                { FieldIdResolver.ToFieldId(v => v.ApplicationMethod), new [] { FieldIdentifiers.ApplicationMethod} },
                { FieldIdResolver.ToFieldId(v => v.ApplicationUrl), new []{ FieldIdentifiers.ApplicationUrl} }
            };

            return new ReviewFieldMappingLookupsForPage(vms, mappings);
        }
    }
}