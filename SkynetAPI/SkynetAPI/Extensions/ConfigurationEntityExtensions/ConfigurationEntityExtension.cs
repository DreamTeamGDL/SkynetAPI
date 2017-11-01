using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.ViewModels.Configuration;
using SkynetAPI.Models.Config;

namespace SkynetAPI.Extensions.ConfigurationEntityExtensions
{
    public static class ConfigurationEntityExtension
    {
        public static ClientConfiguration ToVM(this DynamicTableEntity entity)
        {
            var config = new ClientConfiguration
            {
                ClientName = entity.Properties["ClientName"].StringValue
            };

            foreach (var prop in entity.Properties)
            {
                if(prop.Key != "ClientName")
                {
                    var newKey = prop.Key.Replace("_", " ");
                    config.PinMap.Add(newKey, prop.Value.Int32Value ?? 0);
                }
            }

            return config;
        }

        public static DynamicTableEntity ToEntity(this ClientConfigurationVM config)
        {
            var entity = new DynamicTableEntity
            {
                RowKey = config.MacAddress,
                PartitionKey = config.ZoneId.ToString(),
                Properties = GetProps(config.Configuration)
            };

            return entity;
        }

        private static Dictionary<string, EntityProperty> GetProps(ClientConfiguration configuration)
        {
            var newProps = new Dictionary<string, EntityProperty>
            {
                { nameof(configuration.ClientName), new EntityProperty(configuration.ClientName) }
            };

            foreach (var pin in configuration.PinMap)
            {
                var newKey = pin.Key.Replace(" ", "_");
                newProps.Add(newKey, new EntityProperty(pin.Value));
            }

            return newProps;
        }
    }
}
