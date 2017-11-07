using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkynetAPI.Models
{
    public class Zone
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Client> Clients { get; set; }
        public int ImageIndex { get; set; }
    }
}