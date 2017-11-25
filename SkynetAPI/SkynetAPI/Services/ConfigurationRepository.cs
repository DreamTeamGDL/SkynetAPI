using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

using SkynetAPI.Configs;
using SkynetAPI.Models.Config;
using SkynetAPI.Services.Interfaces;
using SkynetAPI.ViewModels.Configuration;
using SkynetAPI.Extensions.ConfigurationEntityExtensions;

namespace SkynetAPI.Services
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private IQueueService _queueService;
        private CloudTable _configTable;
        public ConfigurationRepository(
            IOptions<TableConfig> options,
            IQueueService queueService)
        {
            var account = CloudStorageAccount.Parse(options.Value.ConnectionString);
            var tableClient = account.CreateCloudTableClient();

            _configTable = tableClient.GetTableReference("Configurations");

            _queueService = queueService;
        }

        public async Task<ClientConfigurationVM> Create(ClientConfigurationVM config)
        {
            var op = TableOperation.Insert(config.ToEntity());
            var result = await _configTable.ExecuteAsync(op);

            return result.HttpStatusCode == 204 ? config : null;
        }

        public async Task<MainConfiguration> Create(MainConfiguration configuration, string userID)
        {
            var entity = new DynamicTableEntity(userID, configuration.MacAddress)
            {
                Properties = new Dictionary<string, EntityProperty>
                {
                    {"ZoneID", new EntityProperty(configuration.ZoneID) }
                }
            };

            var op = TableOperation.Insert(entity);
            var result = await _configTable.ExecuteAsync(op);

            return result.HttpStatusCode == 204 ? configuration : null;
        }

        public async Task<MainConfiguration> Get(string macAddress)
        {
            var query = new TableQuery<DynamicTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, macAddress));

            var results = await _configTable.ExecuteQuerySegmentedAsync(query, null);
            var gotFirst = results?.Results?.First();

            if(gotFirst != null)
                if(gotFirst.Properties.TryGetValue("ZoneID", out var prop))
                    if (prop.GuidValue.HasValue)
                        return new MainConfiguration
                        {
                            ZoneID = prop.GuidValue.Value,
                            MacAddress = gotFirst.RowKey
                        };

            return null;
        }

        public async Task<ClientConfiguration> Get(Guid zoneId, string macAddress)
        {
            var op = TableOperation.Retrieve<DynamicTableEntity>(zoneId.ToString(), macAddress);
            var result = await _configTable.ExecuteAsync(op);

            return result.HttpStatusCode == 200 ? (result.Result as DynamicTableEntity).ToVM() : null;
        }

        public Task<ClientConfigurationVM> Delete(ClientConfigurationVM config) => throw new NotImplementedException();
        
        public async Task<bool> Update(Guid id, string deviceName, int pinNumber)
        {
            var query = new TableQuery()
                .Where(TableQuery.GenerateFilterConditionForGuid("ClientId", QueryComparisons.Equal, id));

            var results = await _configTable.ExecuteQuerySegmentedAsync(query, null);
            var entity = results?.Results?.First();

            entity.Properties.Add(deviceName, new EntityProperty(pinNumber));
            entity.ETag = "*";

            var enqueue = _queueService.Enqueue(entity.ToVM());

            var updateOp = TableOperation.InsertOrReplace(entity);
            var opResult = await _configTable.ExecuteAsync(updateOp);

            await enqueue;

            return opResult.HttpStatusCode == 204;
        }
    }
}
