using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.ViewModels;

namespace SkynetAPI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DevicesController : Controller
    {
        private IDevicesRepository _devicesRepository;
        private IConfigurationRepository _configurationRepository;

        public DevicesController(
            IDevicesRepository devicesRepository,
            IConfigurationRepository configurationRepository)
        {
            _devicesRepository = devicesRepository;
            _configurationRepository = configurationRepository;
        }

        public async Task<IActionResult> Index(string id)
        {
            ViewData["CLIENT_ID"] = id;
            if(Guid.TryParse(id, out var ID))
            {
                return View(await _devicesRepository.GetDevices(ID));
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Create(string id)
        {
            ViewData["CLIENT_ID"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string id, DeviceVM deviceVM)
        {
            if (Guid.TryParse(id, out var ID))
            {
                var deviceId = Guid.NewGuid();
                var normalizedName = deviceId.ToString().Replace("-", "_").Insert(0, "A");

                var changedConfig = await _configurationRepository.Update(ID, normalizedName, deviceVM.PinNumber);
                if (changedConfig)
                {
                    var result = await _devicesRepository.Create(new Models.Device
                    {
                        Name = deviceVM.DeviceName,
                        Id = deviceId
                    }, ID);

                    return RedirectToAction("Index", new { Id = id });
                }
            }

            return BadRequest();
        }
    }
}
