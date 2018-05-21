﻿using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Esfa.Recruit.Vacancies.Client.Domain.Exceptions;
using Esfa.Recruit.Vacancies.Client.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Esfa.Recruit.Vacancies.Client.Application.EventHandlers
{
    public class UpdateVacancyOnReviewCreation : INotificationHandler<VacancyReviewCreatedEvent>
    {
        private readonly IVacancyRepository _repository;

        public UpdateVacancyOnReviewCreation(IVacancyRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(VacancyReviewCreatedEvent notification, CancellationToken cancellationToken)
        {
            var vacancy = await _repository.GetVacancyAsync(notification.VacancyReference);

            if (!vacancy.CanSendForReview)
                throw new InvalidStateException($"Vacancy is not in correct state to be sent for review. Current State: {vacancy.Status}");

            vacancy.Status = VacancyStatus.PendingReview;
            await _repository.UpdateAsync(vacancy);
        }
    }
}