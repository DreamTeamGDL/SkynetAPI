using System.Threading.Tasks;
using System;

using SkynetAPI.Models;
using SkynetAPI.Models.ConnectedDevices;
using SkynetAPI.Services.Interfaces;

namespace SkynetAPI.Services
{
    public class DevicesRepository : IDevicesRepository
    {
        public Task<Device> GetDevice()
        {
            return Task.FromResult(new Device
            {
                Id = Guid.NewGuid(),
                Data = new FanDevice
                {
                    Type = "Fan",
                    Speed = 0.5f,
                    Status = true,
                    Humidity = 0.6f
                }
            });
        }
    }
}