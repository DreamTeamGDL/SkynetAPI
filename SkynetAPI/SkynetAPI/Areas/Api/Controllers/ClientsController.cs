using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.Models;
using SkynetAPI.ViewModels;

namespace SkynetAPI.Areas.Api.Controllers
{
    public class ClientsController : Controller
    {
        private IClientsRepository _clientsRepository;
        public ClientsController(
            IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }



    }
}
