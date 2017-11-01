using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SkynetAPI.Models.Base;
using Newtonsoft.Json.Linq;

namespace SkynetAPI.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public JObject Data { get; set; }
    }   
}