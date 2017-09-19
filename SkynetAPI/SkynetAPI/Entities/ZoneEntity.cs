using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SkynetAPI.Entities
{
    public class ZoneEntity : TableEntity
    {
        public ZoneEntity() { }

        public ZoneEntity(Guid zoneId, string userId)
        {
            this.RowKey = zoneId.ToString();
            this.PartitionKey = userId;
        }

        public string Name { get; set; }
    }
}
