using ElectionAppReact.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectionAppReact.Server.Data
{
    public class PrivatBankDbContext : DbContext, IBankDbContext
    {
        public PrivatBankDbContext(DbContextOptions<PrivatBankDbContext> options)
            : base(options) { }

        public DbSet<BankUser> BankUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankUser>().HasData(
                new BankUser
                {
                    Id = 1,
                    Bank = "privat24",
                    FullName = "Ivan Ivanov",
                    BirthDate = new DateTime(1995, 5, 10),
                    Address = "Kyiv",
                    Password = "1234"
                }
            );
        }
    }
}
