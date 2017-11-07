using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.Models;

namespace SkynetAPI.Services
{
    public class UsersRepository : IUserRepository
    {
        public Task<List<User>> GetUsers() 
            => Task.FromResult(new List<User>()
            {
                new User
                {
                    Name = "Miguel Pérez García",
                    Username = "Miker1423",
                    Id = "59bf3bb07c2eab787c16b3c6"
                },
                new User
                {
                    Name = "Cesar Espinosa",
                    Username = "El del cesar",
                    Id = "59bf3c807c2eab787c16b3e4"
                },
                new User
                {
                    Name = "Chris Marquez",
                    Username = "El christoforo",
                    Id = "59bf3bdf8bd5a32e1089672d"
                }
            });
    }
}
