using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ops.Api.Controllers;
using Ops.Api.Services;
using Xunit;

namespace Ops.Api.Tests.Controllers
{
    public class DeviceControllerTests
    {
        private readonly Mock<IDeviceService> _deviceServiceMock;
        private readonly DeviceController _controller;

        public DeviceControllerTests()
        {
            _deviceServiceMock = new Mock<IDeviceService>();
            _controller = new DeviceController(_deviceServiceMock.Object);
        }

        [Fact]
        public void GetDeviceId_ReturnsOkResult_WithDeviceId()
        {
            // Arrange
            var expectedDeviceId = "test-device-id";
            _deviceServiceMock.Setup(s => s.GetDeviceId()).Returns(expectedDeviceId);

            // Act
            var result = _controller.GetDeviceId();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var response = okResult.Value;
            Assert.NotNull(response);
            var deviceIdProperty = response.GetType().GetProperty("DeviceId");
            Assert.NotNull(deviceIdProperty);
            var actualDeviceId = deviceIdProperty.GetValue(response)?.ToString();
            Assert.Equal(expectedDeviceId, actualDeviceId);
        }

        [Fact]
        public void GetDeviceId_CallsDeviceService_ExactlyOnce()
        {
            // Arrange
            _deviceServiceMock.Setup(s => s.GetDeviceId()).Returns("test-device-id");

            // Act
            var result = _controller.GetDeviceId();

            // Assert
            _deviceServiceMock.Verify(s => s.GetDeviceId(), Times.Once());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetDeviceId_ReturnsOk_WhenDeviceIdIsEmpty()
        {
            // Arrange
            _deviceServiceMock.Setup(s => s.GetDeviceId()).Returns(string.Empty);

            // Act
            var result = _controller.GetDeviceId();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            // checking the property
            var response = okResult.Value;
            Assert.NotNull(response);
            var deviceIdProperty = response?.GetType().GetProperty("DeviceId");
            Assert.NotNull(deviceIdProperty);
            var actualDeviceId = deviceIdProperty.GetValue(response)?.ToString();
            Assert.Equal(string.Empty, actualDeviceId);
        }
    }
}