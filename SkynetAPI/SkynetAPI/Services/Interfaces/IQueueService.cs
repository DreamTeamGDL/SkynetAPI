using SkynetAPI.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkynetAPI.Services.Interfaces
{
    public interface IQueueService
    {
        Task Enqueue(ClientConfiguration message);
    }
}
