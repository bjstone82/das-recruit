﻿using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using FluentValidation;

namespace Esfa.Recruit.Vacancies.Client.Application.Validation.Fluent
{
    public class ApplicationReviewValidator : AbstractValidator<ApplicationReview>
    {
        public const int CandidateFeedbackMaxLength = 200;

        public const string OutcomeRequired = "You must select either successful or unsuccessful";
        public const string CandidateFeedbackRequired = "You must say why the application was unsuccessful";
        public const string CandidateFeedbackLength = "Your feedback must be less than {0} characters";
        public const string CandidateFeedbackFreeTextCharacters = "You have entered invalid characters";
        public const string CandidateFeedbackNull = "You must not provide feedback for a successful application";

        public ApplicationReviewValidator()
        {
            When(x => x.Status == ApplicationReviewStatus.Unsuccessful, () =>
            {
                RuleFor(x => x.CandidateFeedback)
                    .NotEmpty()
                    .WithMessage(CandidateFeedbackRequired)
                    .MaximumLength(CandidateFeedbackMaxLength)
                    .WithMessage(string.Format(CandidateFeedbackLength, CandidateFeedbackMaxLength))
                    .ValidFreeTextCharacters()
                    .WithMessage(CandidateFeedbackFreeTextCharacters);
            });

            When(x => x.Status == ApplicationReviewStatus.Successful, () =>
            {
                RuleFor(x => x.CandidateFeedback)
                    .Empty()
                    .WithMessage(CandidateFeedbackNull);
            });
        }
    }
}
