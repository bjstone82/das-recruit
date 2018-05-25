﻿using Esfa.Recruit.Vacancies.Client.Application.Validation;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Esfa.Recruit.Vacancies.Client.UnitTests.Application.VacancyValidation.SingleField
{
    public class OfflineApplicationUrlValidationTests : VacancyValidationTestsBase
    {
        [Theory]
        [InlineData("applyhere.com")]
        [InlineData("www.applyhere.com")]
        [InlineData("http://www.applyhere.com")]
        [InlineData("https://www.applyhere.com")]
        [InlineData("applyhere.com#anchor")]
        [InlineData("applyhere.com?term=query")]
        public void NoErrorsWhenOfflineApplicationUrlIsValid(string url)
        {
            var vacancy = new Vacancy
            {
                ApplicationUrl = url
            };

            var result = Validator.Validate(vacancy, VacancyRuleSet.ApplicationUrl);

            result.HasErrors.Should().BeFalse();
            result.Errors.Should().HaveCount(0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OfflineApplicationUrlMustHaveAValue(string url)
        {
            var vacancy = new Vacancy
            {
                ApplicationUrl = url
            };

            var result = Validator.Validate(vacancy, VacancyRuleSet.ApplicationUrl);

            result.HasErrors.Should().BeTrue();
            result.Errors[0].PropertyName.Should().Be(nameof(vacancy.ApplicationUrl));
            result.Errors[0].ErrorCode.Should().Be("85");
            result.Errors[0].RuleId.Should().Be((long)VacancyRuleSet.ApplicationUrl);
        }

        [Fact]
        public void OfflineApplicationUrlMustBe200CharactersOrLess()
        {
            var vacancy = new Vacancy
            {
                ApplicationUrl = "http://www.applyhere.com".PadRight(201, 'w')
            };

            var result = Validator.Validate(vacancy, VacancyRuleSet.ApplicationUrl);

            result.HasErrors.Should().BeTrue();
            result.Errors[0].PropertyName.Should().Be(nameof(vacancy.ApplicationUrl));
            result.Errors[0].ErrorCode.Should().Be("84");
            result.Errors[0].RuleId.Should().Be((long)VacancyRuleSet.ApplicationUrl);
        }

        [Theory]
        [InlineData("Invalid Url")]
        [InlineData("applyhere")]
        [InlineData("domain.com ?term=query")]
        public void OfflineApplicationUrlMustBeAValidWebAddress(string invalidUrl)
        {
            var vacancy = new Vacancy
            {
                ApplicationUrl = invalidUrl
            };

            var result = Validator.Validate(vacancy, VacancyRuleSet.ApplicationUrl);

            result.HasErrors.Should().BeTrue();
            result.Errors[0].PropertyName.Should().Be(nameof(vacancy.ApplicationUrl));
            result.Errors[0].ErrorCode.Should().Be("86");
            result.Errors[0].RuleId.Should().Be((long)VacancyRuleSet.ApplicationUrl);
        }
    }
}