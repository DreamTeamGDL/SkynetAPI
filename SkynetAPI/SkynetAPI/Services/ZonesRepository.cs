using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using SkynetAPI.Models;
using SkynetAPI.Services.Interfaces;
using SkynetAPI.DBContext;

namespace SkynetAPI.Services
{
    public class ZonesRepository : IZonesRepository
    {
        private readonly SkynetContext _skynetContext;
        private readonly IClientsRepository _devicesRepository;
        private readonly IUserMapper _userMapper;

        public ZonesRepository(
            IUserMapper userMapper,
            IClientsRepository devicesRepository,
            SkynetContext skynetContext)
        {
            _devicesRepository = devicesRepository;
            _userMapper = userMapper;
            _skynetContext = skynetContext;
        }

        public async Task<bool> CreateZone(Zone zone, Guid userId)
        {
            zone.Id = Guid.NewGuid();
            var relation = new ZoneRelation
            {
                Name = zone.Name,
                UserId = userId,
                ZoneId = zone.Id
            };

            if (await _userMapper.CreateMap(relation))
            {
                await _skynetContext.Zones.AddAsync(zone);
                await _skynetContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public Zone GetZone(string name, Guid userId)
        {
            var zoneId = _userMapper.GetZoneId(userId, name);
            return _skynetContext.Zones.Single(zo => zo.Id == zoneId);
        }

        public IEnumerable<Zone> GetZone(Guid userId)
        {
            var zonesId = _userMapper.GetZonesIds(userId);
            var zones = _skynetContext.Zones
                                      .Where(zone => zonesId.Contains(zone.Id))
                                      .ToList();
            return zones;
        }
    }
}