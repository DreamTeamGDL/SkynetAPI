using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Models.Config;
using SkynetAPI.Services.Interfaces;
using SkynetAPI.ViewModels.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace SkynetAPI.Areas.Api.Controllers
{
    [Area("api")]
    [Route("api/[controller]")]
    [Authorize]
    public class ConfigController : Controller
    {
        private IConfigurationRepository _configurationRepository;
        public ConfigController(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        [HttpGet("{macAddress}")]
        public async Task<IActionResult> Get(string macAddress)
            => Json(await _configurationRepository.Get(macAddress));

        [HttpGet("{id}/{macAddress}")]
        public async Task<IActionResult> Get(string id, string macAddress)
        {
            if (Guid.TryParse(id, out var ID))
            {
                return Json(await _configurationRepository.Get(ID, macAddress));
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ClientConfigurationVM clientConfigurationVM) =>
            Json(await _configurationRepository.Create(clientConfigurationVM));
    }
}
