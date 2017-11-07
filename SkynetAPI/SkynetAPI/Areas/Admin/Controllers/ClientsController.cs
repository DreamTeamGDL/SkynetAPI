using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;

namespace SkynetAPI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ClientsController : Controller
    {
        private IClientsRepository _devicesRepository;
        public ClientsController(IClientsRepository devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }

        public async Task<IActionResult> Index(string id)
        {
            ViewData["USER_ID"] = id;
            if(Guid.TryParse(id, out var ID))
            {
                return View(await _devicesRepository.GetClients(ID));
            }

            return BadRequest();
        }
    }
}
