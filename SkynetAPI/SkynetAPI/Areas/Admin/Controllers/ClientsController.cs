using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.ViewModels;
using SkynetAPI.Models;
using SkynetAPI.Models.Config;
using SkynetAPI.ViewModels.Configuration;

namespace SkynetAPI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ClientsController : Controller
    {
        private IClientsRepository _clientsRepository;
        private IConfigurationRepository _configurationRepository;
        public ClientsController(
            IConfigurationRepository configurationRepository,
            IClientsRepository clientsRepository)
        {
            _configurationRepository = configurationRepository;
            _clientsRepository = clientsRepository;
        }
        
        public async Task<IActionResult> Index(string id)
        {
            ViewData["ZONE_ID"] = id;
            if(Guid.TryParse(id, out var ID))
            {
                return View(await _clientsRepository.GetClients(ID));
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Create(string id)
        {
            ViewData["ZONE_ID"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string id, ClientVM clientConfig)
        {
            var clientID = Guid.NewGuid();
            if(Guid.TryParse(id, out var ID))
            {
                var saved = await _clientsRepository.CreateClient(new Client
                {
                    Id = clientID,
                    Devices = new List<Device>(),
                    Alias = clientConfig.Alias,
                    Name = clientConfig.Name
                }, ID);

                var config = await _configurationRepository.Create(new ClientConfigurationVM
                {
                    MacAddress = clientConfig.MacAddress,
                    ZoneId = ID,
                    Configuration = new ClientConfiguration
                    {
                        ClientId = clientID,
                        ClientName = clientConfig.Name
                    }
                });

                return RedirectToAction("Index", "Devices", new { ID = clientID });
            }

            return BadRequest();
        }
    }
}
