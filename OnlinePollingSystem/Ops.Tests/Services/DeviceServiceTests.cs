using DeviceId;
using Ops.Api.Services;
using Xunit;

namespace Ops.Api.Tests.Services
{
    public class DeviceServiceTests
    {
        private readonly DeviceService _deviceService;

        public DeviceServiceTests()
        {
            _deviceService = new DeviceService();
        }

        [Fact]
        public void GetDeviceId_ReturnsNonEmptyString()
        {
            // Act
            var deviceId = _deviceService.GetDeviceId();

            // Assert
            Assert.NotNull(deviceId);
            Assert.NotEmpty(deviceId);
            Assert.IsType<string>(deviceId);
        }

        [Fact]
        public void GetDeviceId_ReturnsConsistentIdForSameMachine()
        {
            // Act
            var deviceId1 = _deviceService.GetDeviceId();
            var deviceId2 = _deviceService.GetDeviceId();

            // Assert
            Assert.Equal(deviceId1, deviceId2); // Should be the same on the same machine
        }
    }
}