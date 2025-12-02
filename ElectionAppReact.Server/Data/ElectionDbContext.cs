using Microsoft.EntityFrameworkCore;
using ElectionAppReact.Server.Models;

namespace ElectionAppReact.Server.Data
{
    public class ElectionDbContext : DbContext
    {
        public ElectionDbContext(DbContextOptions<ElectionDbContext> options)
            : base(options) { }

        public DbSet<Candidate> Candidates { get; set; }
    }
}
