using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.ViewModels;
using SkynetAPI.Models.Config;

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
            ViewData["USER_ID"] = id;
            if(Guid.TryParse(id, out var ID))
            {
                return View(await _clientsRepository.GetClients(ID));
            }

            return BadRequest();
        }

        public async Task<IActionResult> Create(string zoneID, ClientVM clientConfig)
        {
            var clientID = Guid.NewGuid();
            if(Guid.TryParse(zoneID, out var ID))
            {
                var config = await _configurationRepository.Create(new MainConfiguration
                {
                    ZoneID = clientID,
                    MacAddress = clientConfig.MacAddress
                }, zoneID);


                var saved = await _clientsRepository.CreateClient(new Models.Client
                {
                    Id = clientID,
                    Devices = new List<Models.Device>(),
                    Alias = clientConfig.Alias,
                    Name = clientConfig.Name
                }, ID);

                return RedirectToAction("Index", "Devices", new { ID = clientID });
            }

            return BadRequest();
        }
    }
}
