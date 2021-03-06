﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Esfa.Recruit.Employer.Web.ViewModels.Part1.Wage;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using FluentAssertions;
using Xunit;
using ErrMsg = Esfa.Recruit.Shared.Web.ViewModels.ValidationMessages.WageValidationMessages;

namespace Esfa.Recruit.Employer.UnitTests.Employer.Web.ViewModels.Part1.Wage
{
    public class WageEditModelTests
    {
        public static IEnumerable<object[]> InvalidWageData =>
            new List<object[]>
            {
                new object[] { nameof(WageEditModel.Duration), "1.1", ErrMsg.TypeOfInteger.Duration},

                new object[] { nameof(WageEditModel.WeeklyHours), "27.234", ErrMsg.TypeOfDecimal.WeeklyHours},

                new object[] { nameof(WageEditModel.FixedWageYearlyAmount), "aa", ErrMsg.TypeOfMoney.FixedWageYearlyAmount},
                new object[] { nameof(WageEditModel.FixedWageYearlyAmount), "$15,000.01", ErrMsg.TypeOfMoney.FixedWageYearlyAmount},
                new object[] { nameof(WageEditModel.FixedWageYearlyAmount), "15,000.0135", ErrMsg.TypeOfMoney.FixedWageYearlyAmount}                
            };

        [Theory]
        [MemberData(nameof(InvalidWageData))]
        public void ShouldErrorIfEmployerEditModelIsInvalid(string propertyName, object actualPropertyValue, string expectedErrorMessage)
        {
            //a valid model
            var m = new WageEditModel
            {
                EmployerAccountId = "valid",
                VacancyId = Guid.Parse("53b54daa-4702-4b69-97e5-12123a59f8ad"),
                Duration = "1",
                DurationUnit = DurationUnit.Year,
                WorkingWeekDescription = "valid",
                WeeklyHours = "35.25",
                WageType = WageType.FixedWage,
                FixedWageYearlyAmount = "20,000.43",
                WageAdditionalInformation = "valid"
            };
            
            var context = new ValidationContext(m, null, null);
            var result = new List<ValidationResult>();

            //ensure we are starting with a valid model
            var isInitiallyValid = Validator.TryValidateObject(m, context, result, true);
            isInitiallyValid.Should().BeTrue();

            //set the property we are testing
            m.GetType().GetProperty(propertyName).SetValue(m, actualPropertyValue);

            // Act
            var isValid = Validator.TryValidateObject(m, context, result, true);

            isValid.Should().BeFalse();
            result.Should().HaveCount(1);
            result.Single(r => r.MemberNames.Single() == propertyName).ErrorMessage.Should().Be(expectedErrorMessage);
        }
    }
}

