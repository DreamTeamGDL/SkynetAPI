using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SkynetAPI.ViewModels
{
    public class DeviceVM
    {
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int PinNumber { get; set; }
        public DEVICE_TYPE Type { get; set; }
        public List<SelectListItem> Types { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem{ Text = DEVICE_TYPE.Light.ToString() },
            new SelectListItem{ Text = DEVICE_TYPE.Fan.ToString() },
            new SelectListItem{ Text = DEVICE_TYPE.Camera.ToString() }
        };
    }
}
