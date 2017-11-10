using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkynetAPI.Models.Config;


namespace SkynetAPI.ViewModels.Configuration
{
    public class ClientConfigurationVM
    {
        public string MacAddress { get; set; }
        public Guid ZoneId { get; set; }
        public ClientConfiguration Configuration { get; set; } = new ClientConfiguration();
    }
}
