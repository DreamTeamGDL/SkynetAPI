using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using SkynetAPI.Models;
using SkynetAPI.Models.ConnectedDevices;

using SkynetAPI.Services.Interfaces;

namespace SkynetAPI.Api.Controllers
{
    public class ZonesController : Controller
    {
        private readonly Guid TEST_GUID;
        private readonly IZonesRepository _zonesRepository;

        public ZonesController(IZonesRepository zonesRepository)
        {
            _zonesRepository = zonesRepository;
            TEST_GUID = Guid.Parse("92361e27-34fd-4cc5-893e-22a9b0beaf1f");
        }

        [HttpGet]
        public IActionResult Get() => Json(_zonesRepository.GetZone(TEST_GUID));

        [HttpGet("{name}")]
        public IActionResult Get(string name) => Json(_zonesRepository.GetZone(TEST_GUID));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Zone newZone)
        {
            var zoneCreated = await _zonesRepository.CreateZone(newZone, TEST_GUID);
            if (zoneCreated)
            {
                return RedirectToAction(nameof(this.Get), new { name = newZone.Name });
            }

            return BadRequest(ModelState);
        }
    }
}