using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SkynetAPI.Services.Interfaces;
 
namespace SkynetAPI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class UsersController : Controller
    {
        private IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<IActionResult> Index() 
            => View(await _userRepository.GetUsers());
    }
}
