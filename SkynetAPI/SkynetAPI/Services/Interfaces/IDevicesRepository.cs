using System.Threading.Tasks;
using System;
using System.Collections.Generic;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IDevicesRepository
    {
        Task<Device> GetDevice();
        Task<bool> CreateDevices(List<Device> devices, Guid clientId);
        Task<IEnumerable<Device>> GetDevices(Guid clientId);
    }
}