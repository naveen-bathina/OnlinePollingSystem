using Microsoft.AspNetCore.SignalR;

namespace Ops.Api.Models
{

    public class CreatePollDto 
    {
        public string Question { get; set; }
        public string[] Options { get; set; }
        public DateTime ExpirationDate { get; set; }

    }

    public class PollDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<PollOptionDto> Options { get; set; }
        public bool HasVoted { get; set; }
        public int? VotedOptionId { get; set; }
        public DateTime ExpirationDate { get; set; }

    }

    public class PollOptionDto
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
    }

    public class VoteDto
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public int OptionId { get; set; }
        public string UserIdentifier { get; set; }
    }

    public class CastVoteModel {
        public int OptionId { get; set; }
    }

    public class PollHub : Hub { }
}
