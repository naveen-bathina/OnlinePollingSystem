using Microsoft.EntityFrameworkCore;
using Ops.Api.Repositories.Entities;

namespace Ops.Api.Repositories
{
    public class PollContext : DbContext
    {
        public PollContext(DbContextOptions<PollContext> options) : base(options) { }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
