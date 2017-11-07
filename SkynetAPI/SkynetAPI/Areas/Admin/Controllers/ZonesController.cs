using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.Models;

namespace SkynetAPI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ZonesController : Controller
    {
        private IZonesRepository _zonesRepository;
        public ZonesController(IZonesRepository zonesRepository)
        {
            _zonesRepository = zonesRepository;
        }

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
        public async Task<IActionResult> Create(string id, Zone zone)
        {
            var result = await _zonesRepository.CreateZone(zone, $"auth0|{id}");
            if (result.result)
            {
                return RedirectToAction("index", "clients", new { Id = result.id });
            }

            return BadRequest();
        }
    }
}
