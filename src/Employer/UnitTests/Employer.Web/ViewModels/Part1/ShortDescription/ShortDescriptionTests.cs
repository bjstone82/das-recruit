﻿namespace Esfa.Recruit.Employer.UnitTests.Employer.Web.ViewModels.Part1.ShortDescription
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Esfa.Recruit.Employer.Web.ViewModels.Part1.ShortDescription;
    using FluentAssertions;
    using Xunit;
    using ErrMsg = Esfa.Recruit.Employer.Web.ViewModels.ValidationMessages.ShortDescriptionValidationMessages;

    public class ShortDescriptionTests
    {
        public static IEnumerable<object[]> InvalidShortDescrptionData =>
            new List<object[]>
            {
                new object[] { "NumberOfPositions", null, ErrMsg.Required.NumberOfPositions},
                new object[] { "NumberOfPositions", "0", ErrMsg.Range.NumberOfPositions},
                new object[] { "NumberOfPositions", "3.2", ErrMsg.TypeOfInteger.NumberOfPositions},
                new object[] { "ShortDescription", null, ErrMsg.Required.ShortDescription},
                new object[] { "ShortDescription", new string('a', 49), string.Format(ErrMsg.StringLength.ShortDescription, "ShortDesciprtion", 350, 50)},
                new object[] { "ShortDescription", new string('a', 351), string.Format(ErrMsg.StringLength.ShortDescription, "ShortDesciprtion", 350, 50)},
                new object[] { "ShortDescription", new string('<', 50), ErrMsg.FreeText.ShortDescription},

                new object[] { "EmployerAccountId", null, "The EmployerAccountId field is required."},
                new object[] { "VacancyId", default(Guid), "The field VacancyId is invalid."},
            };

        [Theory]
        [MemberData(nameof(InvalidShortDescrptionData))]
        public void ShouldErrorIfShortDescriptionEditModelIsInvalid(string propertyName, object actualPropertyValue, string expectedErrorMessage)
        {
            //a valid model
            var m = new ShortDescriptionEditModel
            {
                EmployerAccountId = "valid",
                VacancyId = Guid.Parse("53b54daa-4702-4b69-97e5-12123a59f8ad"),
                NumberOfPositions = "1",
                ShortDescription = new string('a', 50)
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
