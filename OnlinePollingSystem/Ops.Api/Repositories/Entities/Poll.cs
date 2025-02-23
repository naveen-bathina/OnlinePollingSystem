namespace Ops.Api.Repositories.Entities
{
    public class Poll
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<PollOption> Options { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class PollOption
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
    }

    public class Vote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public int OptionId { get; set; }
        public string UserIdentifier { get; set; }
    }
}
