using Microsoft.EntityFrameworkCore;
using Ops.Api.Models;
using Ops.Api.Repositories;
using Ops.Api.Repositories.Entities;

namespace Ops.Api.Services
{
    public interface IPollService
    {
        Task<IEnumerable<PollDto>> GetAllPollsAsync(string deviceId);
        Task<PollDto> GetPollAsync(int pollId, string deviceId);
        Task<bool> CreatePollAsync(CreatePollDto createPollModel);
        Task<bool> VoteAsync(int pollId, CastVoteModel vote, string deviceId);
        Task<PollDto> GetPollResultAsync(int pollId, string deviceId);
        Task<bool> ResetPoll(int pollId, string deviceId);
    }


    public class PollService : IPollService
    {
        private ILogger<PollService> _logger;
        private readonly PollContext _context;
        public PollService(ILogger<PollService> logger, PollContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Gets all polls asynchronously.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PollDto>> GetAllPollsAsync(string deviceId)
        {
            var polls = await _context.Polls
                .Include(p => p.Options)
                .ToListAsync();

            var votes = await _context.Votes
                .Where(v => polls.Select(p => p.Id).Contains(v.PollId) && v.UserIdentifier == deviceId)
                .ToListAsync();

            var pollDtos = polls.Select(p => new PollDto
            {
                Id = p.Id,
                Question = p.Question,
                Options = p.Options.Select(o => new PollOptionDto
                {
                    Id = o.Id,
                    OptionText = o.OptionText,
                    VoteCount = o.VoteCount
                }).ToList(),
                HasVoted = votes.Any(v => v.PollId == p.Id), // Check if the user has voted
                VotedOptionId = votes.FirstOrDefault(v => v.PollId == p.Id)?.OptionId, // Get voted option ID if exists
                ExpirationDate = p.ExpirationDate,
            });

            return pollDtos;

        }

        /// <summary>
        /// Creates poll asynchronously.
        /// </summary>
        /// <param name="createPollModel"></param>
        /// <returns></returns>
        public async Task<bool> CreatePollAsync(CreatePollDto createPollModel)
        {
            try
            {
                Poll newPoll = new Poll();
                newPoll.Id = 0;
                newPoll.Question = createPollModel.Question;
                newPoll.Options = createPollModel.Options.Select(x => new PollOption() { Id = 0, OptionText = x, PollId = newPoll.Id, VoteCount = 0 }).ToList();

                _context.Polls.Add(newPoll);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred white creating poll in method CreatePollAsync");
                return false;
            }
        }

        /// <summary>
        /// Cast vote to given poll.
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="vote"></param>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        public async Task<bool> VoteAsync(int pollId, CastVoteModel vote, string userIdentifier)
        {
            if (_context.Votes.Any(v => v.PollId == pollId && v.UserIdentifier == userIdentifier))
                return false;

            var isValidPollOption = _context.Polls.Any(p => p.Id == pollId && p.Options.Any(o => o.Id == vote.OptionId));
            if (!isValidPollOption)
                return false;

            _context.Votes.Add(new Vote() { PollId = pollId, OptionId = vote.OptionId, UserIdentifier = userIdentifier });
            var option = await _context.PollOptions.FindAsync(vote.OptionId);
            if (option != null)
            {
                option.VoteCount++;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// Get poll results asynchronously.
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<PollDto> GetPollResultAsync(int pollId, string deviceId)
        {
            var poll = await _context.Polls
            .Include(p => p.Options)
            .FirstOrDefaultAsync(p => p.Id == pollId);

            if (poll == null)
                return null; // Or throw an exception if preferred

            var votes = await _context.Votes
                .Where(v => v.PollId == pollId && v.UserIdentifier == deviceId)
                .ToListAsync();

            return new PollDto
            {
                Id = poll.Id,
                Question = poll.Question,
                Options = poll.Options.Select(o => new PollOptionDto
                {
                    Id = o.Id,
                    OptionText = o.OptionText,
                    VoteCount = o.VoteCount
                }).ToList(),
                HasVoted = votes.Any(), // If there is at least one vote, the user has voted
                VotedOptionId = votes.FirstOrDefault()?.OptionId, // Get voted option ID if exists
                ExpirationDate = poll.ExpirationDate
            };
        }


        /// <summary>
        /// Get polls asynchronously.
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<PollDto> GetPollAsync(int pollId, string deviceId)
        {
            var poll = await _context.Polls
            .Include(p => p.Options)
            .FirstOrDefaultAsync(p => p.Id == pollId);

            if (poll == null)
                return null; // Or throw an exception if preferred

            var votes = await _context.Votes
                .Where(v => v.PollId == pollId && v.UserIdentifier == deviceId)
                .ToListAsync();

            return new PollDto
            {
                Id = poll.Id,
                Question = poll.Question,
                Options = poll.Options.Select(o => new PollOptionDto
                {
                    Id = o.Id,
                    OptionText = o.OptionText,
                    VoteCount = o.VoteCount
                }).ToList(),
                HasVoted = votes.Any(), // If there is at least one vote, the user has voted
                VotedOptionId = votes.FirstOrDefault()?.OptionId, // Get voted option ID if exists
                ExpirationDate = poll.ExpirationDate
            };
        }

        public async Task<bool> ResetPoll(int pollId, string deviceId)
        {
            var votes = await _context.Votes
            .Where(v => v.PollId == pollId && v.UserIdentifier == deviceId)
            .ToListAsync();

            if (!votes.Any())
            {
                return false;
            }

            // Update vote counts for the options that were voted on
            foreach (var vote in votes)
            {
                var option = await _context.PollOptions.FindAsync(vote.OptionId);
                if (option != null && option.VoteCount > 0)
                {
                    option.VoteCount--;
                }
            }

            // Remove votes from the database
            _context.Votes.RemoveRange(votes);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
