using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ops.Api.Models;
using Ops.Api.Services;

namespace Ops.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly ILogger<PollsController> _logger;
        private readonly IPollService _service;
        private readonly IHubContext<PollHub> _hubContext;
        public PollsController(ILogger<PollsController> logger, IPollService service, IHubContext<PollHub> hubContext)
        {
            _logger = logger;
            _service = service;
            _hubContext = hubContext;
        }


        [HttpPost(Name = "Create poll")]
        public async Task<IActionResult> CreatePoll([FromBody] CreatePollDto poll)
        {
            bool pollCreated = await _service.CreatePollAsync(poll);
            if (pollCreated)
                return Ok("A poll has been created successfully");
            else
                return BadRequest("There is a problem in creating poll, Please try again.");
        }

        [HttpGet(Name = "Get all polls")]
        public async Task<IActionResult> GetPolls()
        {
            if (Request.Headers.TryGetValue("Device-Id", out var deviceId))
            {
                return Ok(await _service.GetAllPollsAsync(deviceId!));
            }
            return BadRequest();
        }


        [HttpGet("{pollId}")]
        public async Task<IActionResult> GetPoll(int pollId)
        {
            if (Request.Headers.TryGetValue("Device-Id", out var deviceId))
            {
                var poll = await _service.GetPollAsync(pollId, deviceId!);
                if (poll != null)
                    return Ok(poll);
                else
                    return NotFound();
            }
            return BadRequest();
        }

        [HttpPost("{pollId}/vote", Name = "vote for poll")]
        public async Task<IActionResult> Vote(int pollId, [FromBody] CastVoteModel vote)
        {
            if (Request.Headers.TryGetValue("Device-Id", out var deviceId))
            {
                if (!(await _service.VoteAsync(pollId, vote, deviceId!)))
                    return BadRequest("User has already voted");

                await _hubContext.Clients.All.SendAsync("ReceivePollUpdate", pollId);
                return Ok("Thank you for voting");
            }
            return BadRequest("User has already voted");
        }

        [HttpGet("{pollId}/results", Name = "Get results for poll")]
        public async Task<IActionResult> GetResults(int pollId)
        {
            if (Request.Headers.TryGetValue("Device-Id", out var deviceId))
            {
                var poll = await _service.GetPollResultAsync(pollId, deviceId!);
                return poll == null ? NotFound() : Ok(poll);
            }
            return BadRequest();
        }

        [HttpPost("{pollId}/reset")]
        public async Task<IActionResult> ResetDeviceVote(int pollId)
        {
            if (Request.Headers.TryGetValue("Device-Id", out var deviceId))
            {
                var reset = await _service.ResetPoll(pollId, deviceId!);
                if (reset)
                    return Ok();

                return BadRequest();
            }
            return BadRequest();
        }
    }
}
