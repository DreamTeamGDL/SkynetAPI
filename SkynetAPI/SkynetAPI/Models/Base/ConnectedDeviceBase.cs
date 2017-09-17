using System;
using System.ComponentModel.DataAnnotations;

namespace SkynetAPI.Models.Base
{
    public class ConnectedDeviceBase
    {
        [Key]
        public Guid ClientId { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
    }
}