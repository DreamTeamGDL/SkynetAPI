using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SkynetAPI.Models.Base;

namespace SkynetAPI.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public dynamic Data { get; set; }
    }   
}