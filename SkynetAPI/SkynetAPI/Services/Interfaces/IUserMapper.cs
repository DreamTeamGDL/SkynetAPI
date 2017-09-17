using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IUserMapper
    {
        List<Guid> GetZonesIds(Guid userId);

        Guid GetZoneId(Guid userId, string zoneName);

        Task<bool> CreateMap(ZoneRelation relation);
    }
}
