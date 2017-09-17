using System;
using System.Collections.Generic;

namespace SkynetAPI.Models
{
    public class Zone
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public List<Client> Clients { get; set; } 
    }
}