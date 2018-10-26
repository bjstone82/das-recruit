using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Infrastructure.EventStore;
using Esfa.Recruit.Vacancies.Client.Infrastructure.ReferenceData;
using Esfa.Recruit.Vacancies.Client.Infrastructure.ReferenceData.BlockedEmployers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Esfa.Recruit.Vacancies.Jobs.NonLevyAccountBlocker
{
    public class NonLevyAccountBlockerJob
    {
        private readonly ILogger<NonLevyAccountBlockerJob> _logger;
        private readonly AccountsReader _accountsReader;
        private readonly IReferenceDataWriter _referenceDataWriter;

        public NonLevyAccountBlockerJob(ILogger<NonLevyAccountBlockerJob> logger, AccountsReader accountsReader, IReferenceDataWriter referenceDataWriter)
        {
            _logger = logger;
            _accountsReader = accountsReader;
            _referenceDataWriter = referenceDataWriter;
        }

#if DEBUG
        public async Task RefreshBlockedEmployerAccounts([QueueTrigger(QueueNames.GenerateBlockedEmployersQueueName, Connection = "EventQueueConnectionString")] string message, TextWriter log)
#else
        public async Task RefreshBlockedEmployerAccounts([TimerTrigger(Schedules.Hourly, RunOnStartup = true)] TimerInfo timerInfo, TextWriter log)
#endif
        {
            _logger.LogInformation("Starting rebuilding blocked employers reference data.");

            var accTask = _accountsReader.GetEmployerAccounts();
            var levyPayersTask = _accountsReader.GetLevyPayerAccountIds();

            await Task.WhenAll(accTask, levyPayersTask);

            var accounts = accTask.Result;
            var levyPayers = levyPayersTask.Result;

            var nonLevyPayers = accounts.Where(x => levyPayers.Contains(x.AccountId) == false)
                                        .Select(x => x.HashedAccountId)
                                        .ToList();

            await _referenceDataWriter.UpsertReferenceData<BlockedEmployers>(new BlockedEmployers
            {
                Id = nameof(BlockedEmployers),
                EmployerAccountIds = nonLevyPayers
            });

            _logger.LogInformation("Finished rebuilding blocked employers reference data.");
        }
    }
}