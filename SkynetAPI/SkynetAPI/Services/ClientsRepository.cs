using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.Models;
using SkynetAPI.Models.ConnectedDevices;

namespace SkynetAPI.Services
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly IDevicesRepository _devicesRepository;
        public ClientsRepository(IDevicesRepository devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }
        public async Task<Client> GetClient()
        {
            return new Client
            {
                Name = "Cocina",
                Alias = "Room1",
                Devices = new List<Device>
                {
                    await _devicesRepository.GetDevice()
                }
            };
        }
    }
}