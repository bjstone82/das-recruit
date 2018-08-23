using System;
using System.IO;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Esfa.Recruit.Vacancies.Jobs.EmployerDashboardGenerator
{
    public class EmployerDashboardGeneratorJob
    {
        private readonly ILogger<EmployerDashboardGeneratorJob> _logger;
        private readonly EmployerDashboardCreator _job;
        private string JobName => GetType().Name;

        public EmployerDashboardGeneratorJob(ILogger<EmployerDashboardGeneratorJob> logger, EmployerDashboardCreator job)
        {
            _logger = logger;
            _job = job;
        }

        public async Task GenerateEmployerVacancyData([QueueTrigger(QueueNames.EmployerDashboardQueueName, Connection = "EventQueueConnectionString")] string message, TextWriter log)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<EmployerDashboardCreateMessage>(message);
                _logger.LogInformation($"Start {JobName} For Employer Account: {data.EmployerAccountId}");
                await _job.RunAsync(data.EmployerAccountId);
                _logger.LogInformation($"Finished {JobName} For Employer Account: {data.EmployerAccountId}");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Unable to deserialise event: {eventBody}", message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to run {JobName}.");
                throw;
            }
        }
    }
}