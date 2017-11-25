using SkynetAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

using SkynetAPI.Configs;
using SkynetAPI.Models;
using SkynetAPI.Models.QueueModels;
using SkynetAPI.Models.Config;

namespace SkynetAPI.Services
{
    public class QueueService : IQueueService
    {
        private CloudQueue cloudQueue;

        public QueueService(IOptions<TableConfig> options)
        {
            var account = CloudStorageAccount.Parse(options.Value.ConnectionString);
            var queueClient = account.CreateCloudQueueClient();
            cloudQueue = queueClient.GetQueueReference("mainqueue");
        }

        public Task Enqueue(ClientConfiguration message)
        {
            var enqueuedMessage = new ActionMessage
            {
                Action = ACTION.CONFIGURE,
                Name = message.ClientId.ToString(),
                Do = JsonConvert.SerializeObject(message)
            };

            return cloudQueue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(enqueuedMessage)));
        }
    }
}
