using Microsoft.AspNetCore.SignalR;

namespace Ops.Api.Models
{
    /// <summary>
    /// Represents a request to create a new poll.
    /// </summary>
    public class CreatePollDto
    {
        /// <summary>
        /// The question for the poll.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// The list of options voters can choose from.
        /// </summary>
        public string[] Options { get; set; }

        /// <summary>
        /// The date and time when the poll expires.
        /// </summary>
        public DateTime ExpirationDate { get; set; }
    }

    /// <summary>
    /// Represents a poll with its details and voting status.
    /// </summary>
    public class PollDto
    {
        /// <summary>
        /// The unique identifier of the poll.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The question posed by the poll.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// The list of options available for voting in the poll.
        /// </summary>
        public List<PollOptionDto> Options { get; set; }

        /// <summary>
        /// Indicates whether the requesting user has already voted in this poll.
        /// </summary>
        public bool HasVoted { get; set; }

        /// <summary>
        /// The ID of the option the user voted for, if applicable; otherwise, null.
        /// </summary>
        public int? VotedOptionId { get; set; }

        /// <summary>
        /// The date and time when the poll expires.
        /// </summary>
        public DateTime ExpirationDate { get; set; }
    }

    /// <summary>
    /// Represents an option within a poll.
    /// </summary>
    public class PollOptionDto
    {
        /// <summary>
        /// The unique identifier of the poll option.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The identifier of the poll this option belongs to.
        /// </summary>
        public int PollId { get; set; }

        /// <summary>
        /// The text describing this voting option.
        /// </summary>
        public string OptionText { get; set; }

        /// <summary>
        /// The number of votes this option has received.
        /// </summary>
        public int VoteCount { get; set; }
    }

    /// <summary>
    /// Represents a vote cast by a user for a poll option.
    /// </summary>
    public class VoteDto
    {
        /// <summary>
        /// The unique identifier of the vote.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The identifier of the poll this vote pertains to.
        /// </summary>
        public int PollId { get; set; }

        /// <summary>
        /// The identifier of the option voted for.
        /// </summary>
        public int OptionId { get; set; }

        /// <summary>
        /// The unique identifier of the user or device casting the vote.
        /// </summary>
        public string UserIdentifier { get; set; }
    }

    /// <summary>
    /// Represents a request to cast a vote for a poll option.
    /// </summary>
    public class CastVoteModel
    {
        /// <summary>
        /// The ID of the option being voted for.
        /// </summary>
        public int OptionId { get; set; }
    }

    /// <summary>
    /// SignalR hub for real-time poll updates.
    /// </summary>
    public class PollHub : Hub
    {
        // No additional methods or properties are defined here, 
        // but this hub can be extended to handle real-time client interactions.
    }
}