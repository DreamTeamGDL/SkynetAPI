using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using SkynetAPI.Models;
using SkynetAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SkynetAPI.Api.Controllers
{
    [Authorize]
    [Area("api")]
    [Route("api/[controller]")]
    public class ZonesController : Controller
    {
        private readonly Guid TEST_GUID;
        private readonly IZonesRepository _zonesRepository;
        private readonly IClientsRepository _clientsRepository;

        public ZonesController(
            IClientsRepository clientsRepository,
            IZonesRepository zonesRepository)
        {
            _clientsRepository = clientsRepository;
            _zonesRepository = zonesRepository;
            TEST_GUID = Guid.Parse("92361e27-34fd-4cc5-893e-22a9b0beaf1f");
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
            => Json(await _zonesRepository.GetZones(User.FindFirst(ClaimTypes.NameIdentifier).Value));
    
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) 
            => Json(await _zonesRepository.GetZone(id, User.FindFirst(ClaimTypes.NameIdentifier).Value));
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Zone newZone)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (newZone != null)
            {
                var zoneCreated = await _zonesRepository.CreateZone(newZone, id);
                if (zoneCreated.result)
                {
                    var clientsResult = await _clientsRepository.CreateClients(newZone.Clients, zoneCreated.id);
                    if (clientsResult)
                    {
                        return RedirectToAction(nameof(this.Get), new { id = newZone.Name });
                    }

                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return BadRequest();
        }
    }
}