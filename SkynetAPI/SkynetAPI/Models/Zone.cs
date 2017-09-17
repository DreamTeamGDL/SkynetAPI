using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkynetAPI.Models
{
    public class Zone
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Client> Clients { get; set; } 
    }
}