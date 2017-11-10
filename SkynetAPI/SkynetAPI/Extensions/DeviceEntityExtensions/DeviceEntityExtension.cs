using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

using SkynetAPI.Models;
using Newtonsoft.Json.Linq;

namespace SkynetAPI.Extensions.DeviceEntityExtensions
{
    public static class DeviceEntityExtension
    {
        public static DynamicTableEntity ToEntity(this Device device, Guid clientId)
        {
            var entity = new DynamicTableEntity
            {
                RowKey = device.Id.ToString(),
                PartitionKey = clientId.ToString(),
                Properties = GetProps(device.Data)
            };

            entity.Properties.Add("name", new EntityProperty(device.Name));

            return entity;
        }

        private static Dictionary<string, EntityProperty> GetProps(JObject data)
        {
            if(data != null)
            {
                var dic = new Dictionary<string, EntityProperty>();
                foreach (var token in data)
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

            return new Dictionary<string, EntityProperty>();
        }
    }
}
