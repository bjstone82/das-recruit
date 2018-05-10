using System;
using Esfa.Recruit.Vacancies.Client.Application.Validation;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Esfa.Recruit.Vacancies.Client.UnitTests.Application.VacancyValidation.SingleField
{
    public class OutcomeDescriptionValidationTests : VacancyValidationTestsBase
    {
        [Fact]
        public void NoErrorsWhenOutcomeDescriptionFieldIsValid()
        {
            var vacancy = new Vacancy 
            {
                OutcomeDescription = "a valid description"
            };

            var result = Validator.Validate(vacancy, VacancyRuleSet.OutcomeDescription);

            result.HasErrors.Should().BeFalse();
            result.Errors.Should().HaveCount(0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OutcomeDescriptionMustNotBeEmpty(string outcomeDescription)
        {
            var vacancy = new Vacancy 
            {
                OutcomeDescription = outcomeDescription
            };

            var result = Validator.Validate(vacancy, VacancyRuleSet.OutcomeDescription);

            result.HasErrors.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be(nameof(vacancy.OutcomeDescription));
            result.Errors[0].ErrorCode.Should().Be("55");
            result.Errors[0].RuleId.Should().Be((long)VacancyRuleSet.OutcomeDescription);
        }

        [Fact]
        public void OutcomeDescriptionMustNotBeLongerThanMaxLength()
        {
            var vacancy = new Vacancy 
            {
                OutcomeDescription = new String('a', 501)
            };

            var result = Validator.Validate(vacancy, VacancyRuleSet.OutcomeDescription);

            result.HasErrors.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be(nameof(vacancy.OutcomeDescription));
            result.Errors[0].ErrorCode.Should().Be("7");
            result.Errors[0].RuleId.Should().Be((long)VacancyRuleSet.OutcomeDescription);
        }

        [Fact]
        public void OutcomeDescriptionMustContainVaildCharacters()
        {
            var vacancy = new Vacancy 
            {
                OutcomeDescription = "<"
            };

            var result = Validator.Validate(vacancy, VacancyRuleSet.OutcomeDescription);

            result.HasErrors.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be(nameof(vacancy.OutcomeDescription));
            result.Errors[0].ErrorCode.Should().Be("6");
            result.Errors[0].RuleId.Should().Be((long)VacancyRuleSet.OutcomeDescription);
        }
    }
}