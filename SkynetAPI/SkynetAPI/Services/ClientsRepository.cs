using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Options;

using SkynetAPI.Models;
using SkynetAPI.Configs;
using SkynetAPI.Entities;
using SkynetAPI.Services.Interfaces;
using SkynetAPI.Extensions.ClientExtensions;

namespace SkynetAPI.Services
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly CloudTable _table;
        private readonly IDevicesRepository _deviceRepository;

        public ClientsRepository(
            IOptions<TableConfig> config,
            IDevicesRepository devicesRepository)
        {
            var account = CloudStorageAccount.Parse(config.Value.ConnectionString);
            var cloudTable = account.CreateCloudTableClient();
            _table = cloudTable.GetTableReference("clients");

            _deviceRepository = devicesRepository;
        }

        public async Task<bool> CreateClient(Client client, Guid zoneId)
        {
            var clientEntity = new ClientEntity(client.Id, zoneId)
            {
                Name = client.Name,
                Alias = client.Alias
            };

            var insertOP = TableOperation.Insert(clientEntity);
            var result = await _table.ExecuteAsync(insertOP);

            return result.HttpStatusCode == 200;
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

        public async Task<bool> UpdateDevice(string name, string action)
        {
            var query = new TableQuery<ClientEntity>()
                .Where(TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, name));

            var results = await _table.ExecuteQuerySegmentedAsync(query, null);
            var clientID = results?.Results?.First()?.RowKey ?? null;

            return await _deviceRepository.Update(clientID, action);
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

                client.Devices = (await _deviceRepository.GetDevices(client.Id)).ToList();

                finalResults.Add(client);
            }

            return finalResults;
        }
    }
}