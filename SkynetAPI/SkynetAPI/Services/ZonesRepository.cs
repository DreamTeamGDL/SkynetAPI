using System.Threading.Tasks;
using System.Collections.Generic;

using SkynetAPI.Models;
using SkynetAPI.Services.Interfaces;

namespace SkynetAPI.Services
{
    public class ZonesRepository : IZonesRepository
    {
        private readonly IClientsRepository _devicesRepository;
        public ZonesRepository(IClientsRepository devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }

        public async Task<Zone> GetZone()
        {
            return new Zone
            {
                Name = "Piso abajo",
                Clients = new List<Client>
                {
                    await _devicesRepository.GetClient()
                }
            };
        }
    }
}