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
using Microsoft.AspNetCore.Hosting;
using SkynetAPI.Configs;

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

        public async Task<bool> CreateDevices(List<Device> devices, Guid clientId)
        {
            var results = new List<IList<TableResult>>();
            if (devices.Count > 100)
            {
                int count = 0;
                do
                {
                    var tempDevices = devices.Skip(count).Take(100).Select(device => 
                    {
                        var entity = new DynamicTableEntity
                        {
                            RowKey = Guid.NewGuid().ToString(),
                            PartitionKey = clientId.ToString(),
                            Properties = GetProps(device.Data)
                        };

                        entity.Properties.Add("name", new EntityProperty(device.Name));

                        return entity;
                    });

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
                var entities = devices.Select(device =>
                {
                    var entity = new DynamicTableEntity
                    {
                        RowKey = Guid.NewGuid().ToString(),
                        PartitionKey = clientId.ToString(),
                        Properties = GetProps(device.Data)
                    };

                    entity.Properties.Add("name", new EntityProperty(device.Name));

                    return entity;
                });

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

            var resultDevices = new List<Device>();
            foreach (var device in devices)
            {
                if(device.Properties.TryGetValue("name", out var Name))
                {
                    var resultDevice = new Device()
                    {
                        Id = Guid.Parse(device.RowKey),
                        Name = Name.StringValue
                    };

                    var dic = new Dictionary<string, object>();
                    foreach (var prop in device.Properties)
                    {
                        if(prop.Key != "name")
                        {
                            dic.Add(prop.Key, prop.Value.PropertyAsObject);
                        }
                    }
                    resultDevice.Data = dic;

                    resultDevices.Add(resultDevice);
                }
            }

            return resultDevices;
        }

        public Task<Device> GetDevice()
        {
            return Task.FromResult(new Device
            {
                Id = Guid.NewGuid(),
                Data = new FanDevice
                {
                    Type = "Fan",
                    Speed = 0.5f,
                    Status = true,
                    Humidity = 0.6f
                }
            });
        }

        private Dictionary<string, EntityProperty> GetProps(dynamic data)
        {
            var dic = new Dictionary<string, EntityProperty>();
            var fanDevice = data as JObject;
            foreach (var token in fanDevice)
            {
                switch (token.Value.Type)
                {
                    case JTokenType.Integer:
                        dic.Add(token.Key, new EntityProperty(token.Value.Value<int>()));
                        break;
                    case JTokenType.Float:
                        dic.Add(token.Key, new EntityProperty(token.Value.Value<float>()));
                        break;
                    case JTokenType.String:
                        dic.Add(token.Key, new EntityProperty(token.Value.Value<string>()));
                        break;
                    case JTokenType.Boolean:
                        dic.Add(token.Key, new EntityProperty(token.Value.Value<bool>()));
                        break;
                    default:
                        break;
                }
            }

            return dic;
        }
    }
}