using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkynetAPI.Models
{
    public class Client
    {
        [Key]
        public Guid ZoneId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public List<Device> Devices { get; set; }
    }
}