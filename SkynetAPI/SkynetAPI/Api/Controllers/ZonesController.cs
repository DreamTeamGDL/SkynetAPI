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
        public IActionResult Get()
        {
            var arr = HttpContext.Request.Path.ToString().Split("/");
            if(arr[arr.Length - 1] != "zones" && arr[arr.Length - 1] != "get")
            {
                return Json(_zonesRepository.GetZone(arr[arr.Length - 1], TEST_GUID));
            }

            return Json(_zonesRepository.GetZone(TEST_GUID));
        }
    
        /*
        [HttpGet("{id}")]
        public IActionResult Get(string id) => Json(_zonesRepository.GetZone(id, TEST_GUID));
        */

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Zone newZone)
        {
            var zoneCreated = await _zonesRepository.CreateZone(newZone, TEST_GUID);
            if (zoneCreated)
            {
                return RedirectToAction(nameof(this.Get), new { id = newZone.Name });
            }

            return BadRequest(ModelState);
        }
    }
}