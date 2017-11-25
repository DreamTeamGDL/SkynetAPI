using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.Models;
using SkynetAPI.ViewModels;
using SkynetAPI.Models.Config;

namespace SkynetAPI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ZonesController : Controller
    {
        private IZonesRepository _zonesRepository;
        private IConfigurationRepository _configurationRepository;
        public ZonesController(
            IZonesRepository zonesRepository,
            IConfigurationRepository configurationRepository)
        {
            _zonesRepository = zonesRepository;
            _configurationRepository = configurationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            ViewData["USER_ID"] = id;
            return View(await _zonesRepository.GetZones($"auth0|{id}"));
        }

        [HttpGet]
        public IActionResult Create(string id)
        {
            ViewData["USER_ID"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string id, ZoneVM zone)
        {
            var zoneID = Guid.NewGuid();
            var result = await _zonesRepository.CreateZone(new Zone
            {
                Name = zone.Name,
                Id = zoneID,
                ImageIndex = zone.ImageIndex
            }, $"auth0|{id}");

            var config = await _configurationRepository.Create(new MainConfiguration
            {
                ZoneID = zoneID,
                MacAddress = zone.MacAddress
            }, id);

            if (result.result)
            {
                return RedirectToAction("index", "clients", new { Id = result.id });
            }

            return BadRequest();
        }
    }
}
