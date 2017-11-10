using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

using SkynetAPI.Models;
using SkynetAPI.Models.ConnectedDevices;
using SkynetAPI.Services.Interfaces;
using SkynetAPI.Configs;
using SkynetAPI.Extensions.DeviceEntityExtensions;

namespace SkynetAPI.Services
{
    public class DevicesRepository : IDevicesRepository
    {
        private readonly CloudTable _table;

        public DevicesRepository(IOptions<TableConfig> config)
        {
            var account = CloudStorageAccount.Parse(config.Value.ConnectionString);
            var client = account.CreateCloudTableClient();
            _table = client.GetTableReference("devices");

            var task = _table.CreateIfNotExistsAsync();
            task.Wait();

            var tableTask = _table.CreateIfNotExistsAsync();
            tableTask.Wait();
        }

        public async Task<bool> Create(Device device, Guid clienId)
        {
            var entity = device.ToEntity(clienId);
            var op = TableOperation.Insert(entity);

            var result = await _table.ExecuteAsync(op);
            return result.HttpStatusCode == 204;
        }

        public async Task<bool> CreateDevices(List<Device> devices, Guid clientId)
        {
            var results = new List<IList<TableResult>>();
            if (devices.Count > 100)
            {
                int count = 0;
                do
                {
                    var tempDevices = devices
                        .Skip(count)
                        .Take(100)
                        .Select(device => device.ToEntity(clientId));

                    var batchOp = new TableBatchOperation();
                    foreach (var device in tempDevices)
                    {
                        batchOp.Insert(device);
                    }

                    var result = await _table.ExecuteBatchAsync(batchOp);
                    results.Add(result);
                    count += tempDevices.Count();
                } while (count != devices.Count);
            }
            else
            {
                var entities = devices.Select(device =>device.ToEntity(clientId));

                var batchOp = new TableBatchOperation();
                foreach (var device in entities)
                {
                    batchOp.Insert(device);
                }

                var result = await _table.ExecuteBatchAsync(batchOp);
                results.Add(result);
            }

            return results.Select(result => result.Where(result2 => result2.HttpStatusCode != 204)).Count() != 0;
        }

        public async Task<IEnumerable<Device>> GetDevices(Guid clientId)
        {
            var query = new TableQuery()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, clientId.ToString()));

            var devices = new List<DynamicTableEntity>();
            TableContinuationToken token = null;
            do
            {
                var result = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = result.ContinuationToken;
                devices.AddRange(result.Results);
            } while (token != null);

            return ProcessResults(devices);
        }

        private List<Device> ProcessResults(List<DynamicTableEntity> devices)
        {
            var resultDevices = new List<Device>();
            foreach (var device in devices)
            {
                if (device.Properties.TryGetValue("name", out var Name))
                {
                    var resultDevice = new Device()
                    {
                        Id = Guid.Parse(device.RowKey),
                        Name = Name.StringValue,
                        Data = new JObject()
                    };

                    var dic = new Dictionary<string, object>();
                    foreach (var prop in device.Properties)
                    {
                        if (prop.Key != "name")
                        {
                            resultDevice.Data.Add(prop.Key, JToken.FromObject(prop.Value.PropertyAsObject));
                        }
                    }

                    resultDevices.Add(resultDevice);
                }
            }

            return resultDevices;
        }
    }
}