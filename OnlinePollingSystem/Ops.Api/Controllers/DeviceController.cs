using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ops.Api.Services;

namespace Ops.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("id")]
        public IActionResult GetDeviceId()
        {
            var deviceId = _deviceService.GetDeviceId();
            return Ok(new { DeviceId = deviceId });
        }
    }
}
