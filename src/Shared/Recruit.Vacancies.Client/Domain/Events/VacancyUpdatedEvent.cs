﻿using System;
using Esfa.Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Esfa.Recruit.Vacancies.Client.Domain.Events
{
    public class VacancyUpdatedEvent : EventBase, INotification, IVacancyEvent
    {
        public string EmployerAccountId { get; set; }
        public Guid VacancyId { get; set; }
    }
}