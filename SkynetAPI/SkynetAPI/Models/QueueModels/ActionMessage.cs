using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkynetAPI.Models.QueueModels
{
    public class ActionMessage
    {
        public ACTION Action { get; set; }
        public string Name { get; set; }
        public string Do { get; set; }
    }
}
