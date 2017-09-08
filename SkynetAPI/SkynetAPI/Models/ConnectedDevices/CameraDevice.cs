using SkynetAPI.Models.Base;

namespace SkynetAPI.Models.ConnectedDevices
{
    public class CameraDevice : ConnectedDeviceBase 
    {
        public object UDPData { get; set; }
    }
}