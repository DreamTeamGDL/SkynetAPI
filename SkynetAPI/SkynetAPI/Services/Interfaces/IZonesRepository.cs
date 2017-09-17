using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IZonesRepository
    {
        IEnumerable<Zone> GetZone(Guid userId);

        Zone GetZone(string name, Guid userId);

        Task<bool> CreateZone(Zone zone, Guid userId);
    }
}