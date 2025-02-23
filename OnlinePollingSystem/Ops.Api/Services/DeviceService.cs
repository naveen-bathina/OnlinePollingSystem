using DeviceId;

namespace Ops.Api.Services
{
    public interface IDeviceService
    {
        string GetDeviceId();
    }

    public class DeviceService : IDeviceService
    {
        public string GetDeviceId()
        {
            return new DeviceIdBuilder()
                .AddMachineName()       // Adds the machine name
                .ToString();            // Generates a unique device ID
        }
    }
}
