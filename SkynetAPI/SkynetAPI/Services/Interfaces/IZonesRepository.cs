using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IZonesRepository
    {
        Task<Zone> GetZone(string name, Guid userId);
        Task<IEnumerable<Zone>> GetZones(Guid userId);
        Task<(bool result, Guid id)> CreateZone(Zone zone, Guid userId);
    }
}