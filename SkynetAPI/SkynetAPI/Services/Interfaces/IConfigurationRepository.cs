using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Models.Config;
using SkynetAPI.ViewModels.Configuration;

namespace SkynetAPI.Services.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<MainConfiguration> Get(string macAddress);

        Task<ClientConfiguration> Get(Guid zoneId, string macAddress);

        Task<bool> Update(Guid id, string deviceName, int pinNumber);

        Task<ClientConfigurationVM> Delete(ClientConfigurationVM config);

        Task<ClientConfigurationVM> Create(ClientConfigurationVM config);

        Task<MainConfiguration> Create(MainConfiguration configuration, string userID);
    }
}
