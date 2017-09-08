using System;

using SkynetAPI.Models.Base;

namespace SkynetAPI.Models
{
    public class Device 
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public ConnectedDeviceBase Data { get; set; }
    }   
}