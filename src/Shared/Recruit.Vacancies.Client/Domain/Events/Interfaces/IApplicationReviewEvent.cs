using System;

namespace Esfa.Recruit.Vacancies.Client.Domain.Events.Interfaces
{
    public interface IApplicationReviewEvent
    {
        string EmployerAccountId { get; }
        Guid VacancyId { get; }
    }
}