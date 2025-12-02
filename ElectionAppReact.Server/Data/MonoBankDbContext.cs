using ElectionAppReact.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectionAppReact.Server.Data
{
    public class MonoBankDbContext : DbContext, IBankDbContext
    {
        public MonoBankDbContext(DbContextOptions<MonoBankDbContext> options)
            : base(options) { }

        public DbSet<BankUser> BankUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankUser>().HasData(
                new BankUser
                {
                    Id = 1,
                    Bank = "monobank",
                    FullName = "Anna Petrenko",
                    BirthDate = new DateTime(2000, 3, 21),
                    Address = "Lviv",
                    Password = "qwerty"
                }
            );
        }
    }
}
