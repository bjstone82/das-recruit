﻿using Esfa.Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Esfa.Recruit.Vacancies.Client.Application.Commands
{
    public class UpdateUserCommand : CommandBase, ICommand, IRequest
    {
        public string EmployerAccountId { get; set; }
    }
}