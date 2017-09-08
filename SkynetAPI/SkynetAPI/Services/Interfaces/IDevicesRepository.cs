using System.Threading.Tasks;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IDevicesRepository
    {
        Task<Device> GetDevice();
    }
}