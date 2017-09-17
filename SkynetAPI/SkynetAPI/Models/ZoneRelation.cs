using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkynetAPI.Models
{
    public class ZoneRelation
    {
        [Key]
        public Guid ZoneId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
    }
}
