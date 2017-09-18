using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Client>> GetClients(Guid zoneId);
        Task<bool> CreateClients(IEnumerable<Client> clients, Guid zoneId);
    }
}