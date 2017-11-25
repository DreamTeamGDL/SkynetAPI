using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.ViewModels.DevicesVM;
using Microsoft.AspNetCore.Authorization;

namespace SkynetAPI.Areas.Api.Controllers
{
    [Area("api")]
    [Route("api/[controller]")]
    [Authorize]
    public class DevicesController : Controller
    {
        private IDevicesRepository _deviceRepository;
        private IClientsRepository _clientsRepository;
        public DevicesController(
            IClientsRepository clientsRepository,
            IDevicesRepository devicesRepository)
        {
            _deviceRepository = devicesRepository;
            _clientsRepository = clientsRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if(Guid.TryParse(id, out var clienID))
            {
                return Json(await _deviceRepository.GetDevices(clienID));
            }

            return BadRequest();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id, [FromBody] DeviceUpdate update)
        {
            if(await _clientsRepository.UpdateDevice(id, update.Action))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
