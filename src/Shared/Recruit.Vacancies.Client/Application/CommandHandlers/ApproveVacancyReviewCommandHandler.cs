﻿using Esfa.Recruit.Vacancies.Client.Application.Commands;
using Esfa.Recruit.Vacancies.Client.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Application.Providers;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Domain.Messaging;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Esfa.Recruit.Vacancies.Client.Application.CommandHandlers
{
    public class ApproveVacancyReviewCommandHandler: IRequestHandler<ApproveVacancyReviewCommand>
    {
        private readonly ILogger<ApproveVacancyReviewCommandHandler> _logger;
        private readonly IVacancyReviewRepository _vacancyReviewRepository;
        private readonly IMessaging _messaging;
        private readonly AbstractValidator<VacancyReview> _vacancyReviewValidator;
        private readonly ITimeProvider _timeProvider;

        public ApproveVacancyReviewCommandHandler(ILogger<ApproveVacancyReviewCommandHandler> logger, 
                                        IVacancyReviewRepository vacancyReviewRepository, 
                                        IMessaging messaging,
                                        AbstractValidator<VacancyReview> vacancyReviewValidator,
                                        ITimeProvider timeProvider)
        {
            _logger = logger;
            _vacancyReviewRepository = vacancyReviewRepository;
            _messaging = messaging;
            _vacancyReviewValidator = vacancyReviewValidator;
            _timeProvider = timeProvider;
        }

        public async Task Handle(ApproveVacancyReviewCommand message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Approving review {reviewId}.", message.ReviewId);

            var review = await _vacancyReviewRepository.GetAsync(message.ReviewId);

            if (!review.CanApprove)
            {
                _logger.LogWarning($"Unable to approve review {{reviewId}} due to review having a status of {review.Status}.", message.ReviewId);
                return;
            }
            
            review.ManualOutcome = ManualQaOutcome.Approved;
            review.Status = ReviewStatus.Closed;
            review.ClosedDate = _timeProvider.Now;
            review.ManualQaComment = message.ManualQaComment;
            review.ManualQaFieldIndicators = message.ManualQaFieldIndicators;

            foreach (var automatedQaOutcomeIndicator in review.AutomatedQaOutcomeIndicators)
            {
                automatedQaOutcomeIndicator.IsReferred = message.SelectedAutomatedQaRuleOutcomeIds
                    .Contains(automatedQaOutcomeIndicator.RuleOutcomeId);
            }

            Validate(review);

            await _vacancyReviewRepository.UpdateAsync(review);

            await _messaging.PublishEvent(new VacancyReviewApprovedEvent
            {
                ReviewId = message.ReviewId,
                VacancyReference = review.VacancyReference
            });
        }

        private void Validate(VacancyReview review)
        {
            var validationResult = _vacancyReviewValidator.Validate(review);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
