using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Models;
using SkynetAPI.Entities;

namespace SkynetAPI.Extensions.ZoneExtensions
{
    public static class ZoneExtension
    {
        public static Zone ToZone(this ZoneEntity entity)
        {
            return new Zone
            {
                Name = entity.Name,
                Id = Guid.Parse(entity.RowKey)
            };
        }
    }
}
