using SkynetAPI.Models.Base;

namespace SkynetAPI.Models.ConnectedDevices
{
    public class FanDevice : ConnectedDeviceBase
    {
        public float Speed { get; set; }
        public float Humidity { get; set; }
        public float Temperature { get; set; }
    }
}