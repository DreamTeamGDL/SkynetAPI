using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkynetAPI.ViewModels
{
    public class DeviceVM
    {
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int PinNumber { get; set; }
    }
}
