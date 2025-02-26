namespace PollingApp.Models
{
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
}
