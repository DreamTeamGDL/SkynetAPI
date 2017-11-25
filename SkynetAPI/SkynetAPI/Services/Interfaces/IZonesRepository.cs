using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using SkynetAPI.Models;
using SkynetAPI.Entities;

namespace SkynetAPI.Services.Interfaces
{
    public interface IZonesRepository
    {
        Task<ZoneEntity> GetZone(string zoneId);
        Task<Zone> GetZone(string name, string userId);
        Task<IEnumerable<Zone>> GetZones(string userId);
        Task<(bool result, Guid id)> CreateZone(Zone zone, string userId);
        Task<bool> UpdateName(string zoneID, string newName);
    }
}