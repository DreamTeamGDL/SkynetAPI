using System.Threading.Tasks;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IClientsRepository
    {
        Task<Client> GetClient();
    }
}