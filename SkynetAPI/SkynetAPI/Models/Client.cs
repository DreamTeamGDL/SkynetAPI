using System.Collections.Generic;

namespace SkynetAPI.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public IEnumerable<Device> Devices { get; set; }
    }
}