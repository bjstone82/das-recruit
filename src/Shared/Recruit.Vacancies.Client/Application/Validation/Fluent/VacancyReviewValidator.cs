﻿using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using FluentValidation;

namespace Esfa.Recruit.Vacancies.Client.Application.Validation.Fluent
{
    public class VacancyReviewValidator : AbstractValidator<VacancyReview>
    {
        public const int ManualQaCommentMaxLength = 5000;

        public const string ManualQaCommentLengthMessage = "Your comment must be less than {0} characters";
        public const string ManualQaCommentFreeTextCharactersMessage = "You have entered invalid characters";

        public VacancyReviewValidator()
        {
            RuleFor(x => x.ManualQaComment)
                .MaximumLength(ManualQaCommentMaxLength)
                .WithMessage(string.Format(ManualQaCommentLengthMessage, ManualQaCommentMaxLength))
                .ValidFreeTextCharacters()
                .WithMessage(ManualQaCommentFreeTextCharactersMessage);
        }
    }
}