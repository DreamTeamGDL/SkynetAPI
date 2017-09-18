using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Entities;
using SkynetAPI.Models;

namespace SkynetAPI.Extensions.ClientExtensions
{
    public static class ClientExtension
    {
        public static Client ToClient(this ClientEntity entity)
        {
            return new Client
            {
                Name = entity.Name,
                Alias = entity.Alias,
                Id = Guid.Parse(entity.RowKey)
            };
        }
    }
}
