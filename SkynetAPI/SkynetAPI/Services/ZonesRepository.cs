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

        public async Task<(bool result, Guid id)> CreateZone(Zone zone, string userId)
        {
            var zoneEntity = new ZoneEntity(zone.Id, userId)
            {
                Name = zone.Name,
                ImageIndex = zone.ImageIndex
            };

            var operation = TableOperation.Insert(zoneEntity);
            var result = await _table.ExecuteAsync(operation);

            return (result.HttpStatusCode == 204, zone.Id);
        }

        public async Task<Zone> GetZone(string name, string userId)
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

        public async Task<ZoneEntity> GetZone(string zoneId)
        {
            var query = new TableQuery<ZoneEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, zoneId));

            var queryResult = await _table.ExecuteQuerySegmentedAsync(query, null);
            var zone = queryResult.Results.First();

            return zone;
        }

        public async Task<IEnumerable<Zone>> GetZones(string userId)
        {
            var query = new TableQuery<ZoneEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));

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

            return results.Select(zone => zone.ToZone());
        }
    }
}