namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Events
{
    public static class QueueNames
    {
        public const string VacancyEventsQueueName = "vacancy-events-queue";
        public const string VacancyReviewEventsQueueName = "vacancy-review-events-queue";
        public const string EmployerDashboardQueueName = "generate-employer-dashboard-queue";
        public const string GenerateLiveVacanciesQueueName = "generate-live-vacancies-queue";
        public const string ApplicationSubmittedQueueName = "application-submitted-queue";
        public const string VacancyIndexQueueName = "recruit-index-vacancy-queue";
    }
}