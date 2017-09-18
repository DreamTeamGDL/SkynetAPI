using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.Extensions.ClientExtensions;
using SkynetAPI.Models;
using SkynetAPI.Models.ConnectedDevices;
using SkynetAPI.DBContext;
using SkynetAPI.Entities;
using Microsoft.AspNetCore.Hosting;

namespace SkynetAPI.Services
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly CloudTable _table;
        private readonly IDevicesRepository _deviceRepository;

        public ClientsRepository(IDevicesRepository devicesRepository, IHostingEnvironment env)
        {
            _deviceRepository = devicesRepository;
            if (env.IsDevelopment())
            {
                var account = CloudStorageAccount.DevelopmentStorageAccount;
                var cloudTableClient = account.CreateCloudTableClient();
                _table = cloudTableClient.GetTableReference("clients");
            }
            else
            {
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=skynetgdl;AccountKey=KVJGcGdkiUg6rhDyDbvbgb5YfCf3zaQX3z78K5YFrW4zmjaGzAnUlZwCna4k7nhuq9sZU6uqb7dHdi3S5EODvw==;EndpointSuffix=core.windows.net");
                var cloudTableClient = account.CreateCloudTableClient();
                _table = cloudTableClient.GetTableReference("clients");
            }

            var tableTask = _table.CreateIfNotExistsAsync();
            tableTask.Wait();
        }

        public async Task<bool> CreateClients(IEnumerable<Client> clients, Guid zoneId)
        {
            var results = new List<IList<TableResult>>();
            var clientCount = clients.Count();
            var clientsIds = new List<Guid>();

            if(clientCount > 100)
            {
                int i = 0;
                while (clientCount != i)
                {
                    var batchOp = new TableBatchOperation();

                    var tempClients = clients.Skip(i).Take(100);
                    var entities = tempClients.Select(client => 
                    {
                        var id = Guid.NewGuid();
                        clientsIds.Add(id);
                        return new ClientEntity(id, zoneId)
                        {
                            Alias = client.Alias,
                            Name = client.Name
                        };
                    });

                    foreach (var entity in entities)
                    {
                        batchOp.Insert(entity);
                    }

                    results.Add(await _table.ExecuteBatchAsync(batchOp));
                    i += tempClients.Count();
                }
            }
            else
            {
                var batchOp = new TableBatchOperation();
                var entities = clients.Select(client => 
                {
                    var id = Guid.NewGuid();
                    clientsIds.Add(id);
                    return new ClientEntity(id, zoneId)
                    {
                        Name = client.Name,
                        Alias = client.Alias
                    };
                });

                foreach (var entity in entities)
                {
                    batchOp.Insert(entity);
                }

                results.Add(await _table.ExecuteBatchAsync(batchOp));
            }

            for (int i = 0; i < clientCount; i++)
            {
                await _deviceRepository.CreateDevices(clients.ElementAt(i).Devices, clientsIds[i]);
            }


            return results.Select(result => result.Where(result2 => result2.HttpStatusCode != 204)).Count() != 0;
        }

        public async Task<IEnumerable<Client>> GetClients(Guid zoneId)
        {
            var query = new TableQuery<ClientEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, zoneId.ToString()));

            var clients = new List<ClientEntity>();

            TableContinuationToken token = null;
            do
            {
                var results = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = results.ContinuationToken;
                clients.AddRange(results.Results);
            } while (token != null);

            var finalResults = new List<Client>();
            for (int i = 0; i < clients.Count; i++)
            {
                var client = clients[i].ToClient();
                client.Devices = (await _deviceRepository.GetDevices(Guid.Parse(clients[i].RowKey))).ToList();

                finalResults.Add(client);
            }

            return finalResults;
        }
    }
}