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
        Task<ClientConfiguration> Get(Guid zoneId, string macAddress);

        Task<ClientConfigurationVM> Update(ClientConfigurationVM config);

        Task<ClientConfigurationVM> Delete(ClientConfigurationVM config);

        Task<ClientConfigurationVM> Create(ClientConfigurationVM config);
    }
}
