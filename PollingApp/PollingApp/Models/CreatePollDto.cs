namespace PollingApp.Models
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
}
