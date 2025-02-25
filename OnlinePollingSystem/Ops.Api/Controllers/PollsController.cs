using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ops.Api.Models;
using Ops.Api.Services;

namespace Ops.Api.Controllers
{
    /// <summary>
    /// Manages poll creation, voting, and results.
    /// </summary>
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

        /// <summary>
        /// Creates a new poll.
        /// </summary>
        /// <param name="poll">The poll details to create.</param>
        /// <returns>A confirmation message if successful.</returns>
        /// <response code="200">Poll created successfully.</response>
        /// <response code="400">Failed to create the poll.</response>
        [HttpPost(Name = "Create poll")]
        public async Task<IActionResult> CreatePoll([FromBody] CreatePollDto poll)
        {
            bool pollCreated = await _service.CreatePollAsync(poll);
            if (pollCreated)
                return Ok("A poll has been created successfully");
            else
                return BadRequest("There is a problem in creating poll, Please try again.");
        }

        /// <summary>
        /// Retrieves all polls for the device.
        /// </summary>
        /// <remarks>
        /// Requires a "Device-Id" header to identify the requesting device.
        /// </remarks>
        /// <returns>A list of polls.</returns>
        /// <response code="200">Returns the list of polls.</response>
        /// <response code="400">Missing or invalid Device-Id header.</response>
        [HttpGet(Name = "Get all polls")]
        public async Task<IActionResult> GetPolls()
        {
            if (Request.Headers.TryGetValue("Device-Id", out var deviceId))
            {
                return Ok(await _service.GetAllPollsAsync(deviceId!));
            }
            return BadRequest();
        }

        /// <summary>
        /// Retrieves a specific poll by ID.
        /// </summary>
        /// <param name="pollId">The ID of the poll to retrieve.</param>
        /// <returns>The poll details.</returns>
        /// <response code="200">Poll found and returned.</response>
        /// <response code="404">Poll not found.</response>
        /// <response code="400">Missing or invalid Device-Id header.</response>
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

        /// <summary>
        /// Casts a vote for a poll.
        /// </summary>
        /// <param name="pollId">The ID of the poll to vote on.</param>
        /// <param name="vote">The vote details (option ID).</param>
        /// <returns>A confirmation message if successful.</returns>
        /// <response code="200">Vote cast successfully.</response>
        /// <response code="400">User has already voted or invalid Device-Id.</response>
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

        /// <summary>
        /// Retrieves the results of a poll.
        /// </summary>
        /// <param name="pollId">The ID of the poll to get results for.</param>
        /// <returns>The poll results.</returns>
        /// <response code="200">Poll results returned.</response>
        /// <response code="404">Poll not found.</response>
        /// <response code="400">Missing or invalid Device-Id header.</response>
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

        /// <summary>
        /// Resets the device's vote for a poll.
        /// </summary>
        /// <param name="pollId">The ID of the poll to reset the vote for.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="200">Vote reset successfully.</response>
        /// <response code="400">Reset failed or invalid Device-Id.</response>
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
