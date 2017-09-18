using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkynetAPI.Entities
{
    public class ClientEntity : TableEntity
    {
        public ClientEntity() { }

        public ClientEntity(Guid clientId, Guid zoneId)
        {
            this.RowKey = clientId.ToString();
            this.PartitionKey = zoneId.ToString();
        }

        public string Name { get; set; }
        public string Alias { get; set; }
    }
}
