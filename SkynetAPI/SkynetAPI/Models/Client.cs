using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkynetAPI.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public List<Device> Devices { get; set; }
    }
}