using System.Collections.Generic;
using System.Reflection.Emit;
using ElectionAppReact.Server.Models;
using Microsoft.EntityFrameworkCore;

public class BankDbContext : DbContext
{
    public DbSet<BankUser> BankUsers { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }

    public BankDbContext(DbContextOptions<BankDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AdminUser>().HasData(
            new AdminUser
            {
                Id = 1,
                FullName = "Marta Mykh",
                Password = "admin123"
            }
        );
    }
}
