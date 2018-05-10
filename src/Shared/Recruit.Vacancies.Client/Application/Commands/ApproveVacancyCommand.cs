﻿using System;
using Esfa.Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Esfa.Recruit.Vacancies.Client.Application.Commands
{
    public class ApproveVacancyCommand : CommandBase, ICommand, IRequest
    {
        public long VacancyReference { get; set; }
    }
}