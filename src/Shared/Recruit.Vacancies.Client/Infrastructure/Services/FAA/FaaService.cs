﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Services.FAA
{
    public class FaaService : IFaaService
    {
        private const string Topic = "UpdateApprenticeshipApplicationStatus";
        private readonly FaaConfiguration _config;

        public FaaService(IOptions<FaaConfiguration> config)
        {
            _config = config.Value;
        }

        public Task PublishApplicationStatusSummaryAsync(FaaApplicationStatusSummary message)
        {
            var topicClient = new TopicClient(_config.StorageConnectionString, Topic, RetryPolicy.Default);

            var brokeredMessage = CreateBrokeredMessage(message);

            return topicClient.SendAsync(brokeredMessage);
        }

        private static Message CreateBrokeredMessage<T>(T message) where T : class
        {
            var json = JsonConvert.SerializeObject(message);

            //Creates a message that is compatible with .NET 4.5 BrokeredMessage subscribers
            Message brokeredMessage;
            var ser = new DataContractSerializer(typeof(string));
            using (var ms = new MemoryStream())
            {
                var binaryDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(ms);
                ser.WriteObject(binaryDictionaryWriter, json);
                binaryDictionaryWriter.Flush();
                brokeredMessage = new Message(ms.ToArray());
            }
            
            return brokeredMessage;
        }
    }
}

