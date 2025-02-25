using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using Ops.Api.Controllers;
using Ops.Api.Models;
using Ops.Api.Services;

namespace Ops.Api.Tests.Controllers
{
    public class PollsControllerTests
    {
        private readonly Mock<ILogger<PollsController>> _loggerMock;
        private readonly Mock<IPollService> _pollServiceMock;
        private readonly Mock<IHubContext<PollHub>> _hubContextMock;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly PollsController _controller;

        public PollsControllerTests()
        {
            _loggerMock = new Mock<ILogger<PollsController>>();
            _pollServiceMock = new Mock<IPollService>();
            _hubContextMock = new Mock<IHubContext<PollHub>>();
            _hubClientsMock = new Mock<IHubClients>();
            _clientProxyMock = new Mock<IClientProxy>();

            // Setup hub context and clients
            _hubContextMock.Setup(h => h.Clients).Returns(_hubClientsMock.Object);
            _hubClientsMock.Setup(c => c.All).Returns(_clientProxyMock.Object);

            _controller = new PollsController(_loggerMock.Object, _pollServiceMock.Object, _hubContextMock.Object);

            // Setup default HttpContext with Device-Id header
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Device-Id"] = "test-device-id";
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        }

        [Fact]
        public async Task CreatePoll_ReturnsOk_WhenPollCreated()
        {
            // Arrange
            var pollDto = new CreatePollDto { Question = "Test Poll", Options = new List<string> { "Opt1" }.ToArray() };
            _pollServiceMock.Setup(s => s.CreatePollAsync(pollDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreatePoll(pollDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("A poll has been created successfully", okResult.Value);
        }

        [Fact]
        public async Task CreatePoll_ReturnsBadRequest_WhenPollCreationFails()
        {
            // Arrange
            var pollDto = new CreatePollDto { Question = "Test Poll", Options = new List<string> { "Opt1" }.ToArray() };
            _pollServiceMock.Setup(s => s.CreatePollAsync(pollDto)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreatePoll(pollDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("There is a problem in creating poll, Please try again.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetPolls_ReturnsOk_WithPolls_WhenDeviceIdPresent()
        {
            // Arrange
            var polls = new List<PollDto> { new PollDto { Id = 1, Question = "Test Poll" } };
            _pollServiceMock.Setup(s => s.GetAllPollsAsync("test-device-id")).ReturnsAsync(polls);

            // Act
            var result = await _controller.GetPolls();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(polls, okResult.Value);
        }

        [Fact]
        public async Task GetPolls_ReturnsBadRequest_WhenDeviceIdMissing()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Request.Headers.Remove("Device-Id");

            // Act
            var result = await _controller.GetPolls();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetPoll_ReturnsOk_WhenPollExists()
        {
            // Arrange
            var poll = new PollDto { Id = 1, Question = "Test Poll" };
            _pollServiceMock.Setup(s => s.GetPollAsync(1, "test-device-id")).ReturnsAsync(poll);

            // Act
            var result = await _controller.GetPoll(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(poll, okResult.Value);
        }

        [Fact]
        public async Task GetPoll_ReturnsNotFound_WhenPollDoesNotExist()
        {
            // Arrange
            _pollServiceMock.Setup(s => s.GetPollAsync(1, "test-device-id")).ReturnsAsync((PollDto)null);

            // Act
            var result = await _controller.GetPoll(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Vote_ReturnsOk_WhenVoteSucceeds()
        {
            // Arrange
            var vote = new CastVoteModel { OptionId = 1 };
            _pollServiceMock.Setup(s => s.VoteAsync(1, vote, "test-device-id")).ReturnsAsync(true);
            _clientProxyMock.Setup(c => c.SendCoreAsync("ReceivePollUpdate", It.Is<object[]>(args => args.Length == 1 && (int)args[0] == 1), It.IsAny<CancellationToken>()))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Vote(1, vote);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Thank you for voting", okResult.Value);
            _clientProxyMock.Verify(c => c.SendCoreAsync("ReceivePollUpdate", It.Is<object[]>(args => args.Length == 1 && (int)args[0] == 1), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Vote_ReturnsBadRequest_WhenUserAlreadyVoted()
        {
            // Arrange
            var vote = new CastVoteModel { OptionId = 1 };
            _pollServiceMock.Setup(s => s.VoteAsync(1, vote, "test-device-id")).ReturnsAsync(false);

            // Act
            var result = await _controller.Vote(1, vote);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("User has already voted", badRequestResult.Value);
        }

        [Fact]
        public async Task GetResults_ReturnsOk_WhenPollExists()
        {
            // Arrange
            var poll = new PollDto { Id = 1, Question = "Test Poll" };
            _pollServiceMock.Setup(s => s.GetPollResultAsync(1, "test-device-id")).ReturnsAsync(poll);

            // Act
            var result = await _controller.GetResults(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(poll, okResult.Value);
        }

        [Fact]
        public async Task GetResults_ReturnsNotFound_WhenPollDoesNotExist()
        {
            // Arrange
            _pollServiceMock.Setup(s => s.GetPollResultAsync(1, "test-device-id")).ReturnsAsync((PollDto)null);

            // Act
            var result = await _controller.GetResults(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task ResetDeviceVote_ReturnsOk_WhenResetSucceeds()
        {
            // Arrange
            _pollServiceMock.Setup(s => s.ResetPoll(1, "test-device-id")).ReturnsAsync(true);

            // Act
            var result = await _controller.ResetDeviceVote(1);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task ResetDeviceVote_ReturnsBadRequest_WhenResetFails()
        {
            // Arrange
            _pollServiceMock.Setup(s => s.ResetPoll(1, "test-device-id")).ReturnsAsync(false);

            // Act
            var result = await _controller.ResetDeviceVote(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task ResetDeviceVote_ReturnsBadRequest_WhenDeviceIdMissing()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Request.Headers.Remove("Device-Id");

            // Act
            var result = await _controller.ResetDeviceVote(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
    }
}