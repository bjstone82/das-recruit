﻿namespace Esfa.Recruit.Vacancies.Client.Application.Exceptions
{
    public static class ExceptionMessages
    {
        public const string RouteNotValidForVacancy = "Invalid route for vacancy. Requested route:{0} Redirecting to route:{1}";
        public const string VacancyUnauthorisedAccess = "The employer account {0} cannot access employer account {1} vacancy '{2} ({3})'.";
    }
}
