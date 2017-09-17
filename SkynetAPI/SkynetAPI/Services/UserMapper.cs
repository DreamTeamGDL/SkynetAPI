using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Models;
using SkynetAPI.Services.Interfaces;
using SkynetAPI.DBContext;

namespace SkynetAPI.Services
{
    public class UserMapper : IUserMapper
    {
        private readonly SkynetContext _skynetContext;

        public UserMapper(SkynetContext skynetContext)
        {
            _skynetContext = skynetContext;
        }

        public async Task<bool> CreateMap(ZoneRelation relation)
        {
            await _skynetContext.ZonesRelation.AddAsync(relation);
            var i = await _skynetContext.SaveChangesAsync();
            if(i != 0)
            {
                return true;
            }

            return false;
        }

        public List<Guid> GetZonesIds(Guid userId) 
            => _skynetContext.ZonesRelation.Where(zone => zone.UserId == userId).Select(zone => zone.ZoneId).ToList();

        public Guid GetZoneId(Guid userId, string zoneName)
        {
            var zoneId = _skynetContext.ZonesRelation.Single(zone => zone.UserId == userId && zone.Name == zoneName);
            return zoneId.ZoneId;
        }
    }
}
