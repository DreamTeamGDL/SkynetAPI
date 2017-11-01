using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkynetAPI.Models.Config
{
    public class ClientConfiguration
    {
        public string ClientName { get; set; }
        public Dictionary<string, int> PinMap { get; set; } = new Dictionary<string, int>();
    }
}
