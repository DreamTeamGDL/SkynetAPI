using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
using SkynetAPI.Models;
using SkynetAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace SkynetAPI.Areas.Api.Controllers
{
    [Area("api")]
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        private IClientsRepository _clientsRepository;
        public ClientsController(
            IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if(Guid.TryParse(id, out var zoneID))
            {
                return Json(await _clientsRepository.GetClients(zoneID));
            }

            return BadRequest();
        }
    }
}
