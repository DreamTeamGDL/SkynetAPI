using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Models;

namespace SkynetAPI.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
    }
}
