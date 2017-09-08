using System.Collections.Generic;

namespace SkynetAPI.Models
{
    public class Zone
    {
        public string Name { get; set; }
        public IEnumerable<Client> Clients { get; set; } 
    }
}