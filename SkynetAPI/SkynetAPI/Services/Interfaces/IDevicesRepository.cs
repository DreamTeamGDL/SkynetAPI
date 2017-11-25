using System.Threading.Tasks;
using System;
using System.Collections.Generic;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IDevicesRepository
    {
        Task<bool> CreateDevices(List<Device> devices, Guid clientId);
        Task<bool> Create(Device device, Guid clienId, DEVICE_TYPE type);
        Task<IEnumerable<Device>> GetDevices(Guid clientId);
        Task<bool> Update(string deviceName, string update);
    }
}