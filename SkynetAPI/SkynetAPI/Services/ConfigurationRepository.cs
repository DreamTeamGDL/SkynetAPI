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
        private CloudTable _configTable;
        public ConfigurationRepository(IOptions<TableConfig> options)
        {
            var account = CloudStorageAccount.Parse(options.Value.ConnectionString);
            var tableClient = account.CreateCloudTableClient();

            _configTable = tableClient.GetTableReference("Configurations");
            var tableTask = _configTable.CreateIfNotExistsAsync();
            tableTask.Wait();

        }

        public async Task<ClientConfigurationVM> Create(ClientConfigurationVM config)
        {
            var op = TableOperation.Insert(config.ToEntity());
            var result = await _configTable.ExecuteAsync(op);

            return result.HttpStatusCode == 204 ? config : null;
        }

        public async Task<ClientConfiguration> Get(Guid zoneId, string macAddress)
        {
            var op = TableOperation.Retrieve<DynamicTableEntity>(zoneId.ToString(), macAddress);
            var result = await _configTable.ExecuteAsync(op);

            return result.HttpStatusCode == 200 ? (result.Result as DynamicTableEntity).ToVM() : null;
        }

        public Task<ClientConfigurationVM> Delete(ClientConfigurationVM config) => throw new NotImplementedException();
        public Task<ClientConfigurationVM> Update(ClientConfigurationVM config) => throw new NotImplementedException();
    }
}
