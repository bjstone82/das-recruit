using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Application.Configuration;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Mongo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;

namespace Recruit.Vacancies.Client.Infrastructure.Configuration
{
    internal sealed class ConfigurationReader : MongoDbCollectionBase, IConfigurationReader
    {
        public ConfigurationReader(ILoggerFactory loggerFactory, IOptions<MongoDbConnectionDetails> details)
            : base(loggerFactory, MongoDbNames.RecruitDb, MongoDbCollectionNames.Configuration, details)
        {
        }

        public async Task<T> GetAsync<T>(string id) where T : class
        {
            var filter = Builders<T>.Filter.Eq("_id", id);

            var collection = GetCollection<T>();
            var result = await RetryPolicy.ExecuteAsync(context => collection.FindAsync(filter), new Context(nameof(GetAsync)));

            return result?.FirstOrDefault();
        }
    }
}