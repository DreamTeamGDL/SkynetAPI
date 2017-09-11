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
        private readonly IZonesRepository _zonesRepository;

        public ZonesController(IZonesRepository zonesRepository)
        {
            _zonesRepository = zonesRepository;
        }
        
        public async Task<IActionResult> Get() => Json(await _zonesRepository.GetZone());
    }
}