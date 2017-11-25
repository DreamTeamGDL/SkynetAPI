using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkynetAPI.Models
{
    public enum ACTION
    {
        CONNECT,
        TELL,
        HELLO,
        CONFIGURE
    }

    public enum DEVICE_TYPE
    {
        Fan,
        Light,
        Camera
    }
}
