using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;

namespace SkynetAPI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DevicesController : Controller
    {
        private IDevicesRepository _devicesRepository;
        public DevicesController(IDevicesRepository devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }

        public async Task<IActionResult> Index(string id)
        {
            ViewData["USER_ID"] = id;
            if(Guid.TryParse(id, out var ID))
            {
                return View(await _devicesRepository.GetDevices(ID));
            }

            return BadRequest();
        }
    }
}
