namespace PollingApp.Models
{
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
}
