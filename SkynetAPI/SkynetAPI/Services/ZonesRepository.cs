using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Options;

using SkynetAPI.Models;
using SkynetAPI.Services.Interfaces;
using SkynetAPI.Entities;
using SkynetAPI.Extensions.ZoneExtensions;
using Microsoft.AspNetCore.Hosting;
using SkynetAPI.Configs;

namespace SkynetAPI.Services
{
    public class ZonesRepository : IZonesRepository
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly CloudTable _table;

        public ZonesRepository(
            IOptions<TableConfig> config,
            IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;

            var cloudClient = CloudStorageAccount.Parse(config.Value.ConnectionString);
            var tableClient = cloudClient.CreateCloudTableClient();
            _table = tableClient.GetTableReference("zones");

            var task = _table.CreateIfNotExistsAsync();
            task.Wait();
        }

        public async Task<(bool result, Guid id)> CreateZone(Zone zone, Guid userId)
        {
            var zoneId = Guid.NewGuid();
            var zoneEntity = new ZoneEntity(zoneId, userId)
            {
                Name = zone.Name
            };

            var operation = TableOperation.Insert(zoneEntity);
            var result = await _table.ExecuteAsync(operation);

            return (result.HttpStatusCode == 204, zoneId);
        }

        public async Task<Zone> GetZone(string name, Guid userId)
        {
            var query = new TableQuery<ZoneEntity>()
                .Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId.ToString()),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, name)));
            
            var queryResult = await _table.ExecuteQuerySegmentedAsync(query, null);

            var zone = queryResult.Results.First().ToZone();
            zone.Clients = (await _clientsRepository.GetClients(Guid.Parse(queryResult.First().RowKey))).ToList();

            return zone;
        }

        public async Task<IEnumerable<Zone>> GetZones(Guid userId)
        {
            var query = new TableQuery<ZoneEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId.ToString()));

            var results = new List<ZoneEntity>();

            TableContinuationToken token = null;
            do
            {
                var queryResults = await _table.ExecuteQuerySegmentedAsync(query, token);
                results.AddRange(queryResults.Results);
                token = queryResults.ContinuationToken;
            } while (token != null);

            var zones = new List<Zone>();
            for (int i = 0; i < results.Count; i++)
            {
                var zone = results[i].ToZone();
                zone.Clients = (await _clientsRepository.GetClients(Guid.Parse(results[i].RowKey))).ToList();

                zones.Add(zone);
            }

            return zones;
        }
    }
}