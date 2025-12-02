using ElectionAppReact.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectionAppReact.Server.Data
{
    public class OschadBankDbContext : DbContext, IBankDbContext
    {
        public OschadBankDbContext(DbContextOptions<OschadBankDbContext> options)
            : base(options) { }

        public DbSet<BankUser> BankUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankUser>().HasData(
                new BankUser
                {
                    Id = 1,
                    Bank = "oschadbank",
                    FullName = "Petro Shafarenko",
                    BirthDate = new DateTime(1988, 11, 5),
                    Address = "Odessa",
                    Password = "oschadpass"
                }
            );
        }
    }
}
