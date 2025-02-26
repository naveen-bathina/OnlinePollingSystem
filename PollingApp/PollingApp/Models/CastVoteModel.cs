namespace PollingApp.Models
{
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
}
