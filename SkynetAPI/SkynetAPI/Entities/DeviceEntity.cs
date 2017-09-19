using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkynetAPI.Entities
{
    public class DeviceEntity : TableEntity
    {
        public DeviceEntity(Guid deviceId, Guid clientId)
        {
            this.RowKey = deviceId.ToString();
            this.PartitionKey = clientId.ToString();
        }

        public string Name { get; set; }
    }
}
