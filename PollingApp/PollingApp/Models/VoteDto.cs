namespace PollingApp.Models
{
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
}
