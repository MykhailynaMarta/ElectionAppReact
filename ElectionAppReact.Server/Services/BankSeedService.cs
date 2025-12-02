using ElectionAppReact.Server.Data;

using ElectionAppReact.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectionAppReact.Server.Services
{
    public class BankSeedService
    {
        private readonly IBankDbContextFactory _factory;

        public BankSeedService(IBankDbContextFactory factory)
        {
            _factory = factory;
        }

        public async Task SeedAllAsync()
        {
            //await SeedBank("privat24", BankSeedData.PrivatUsers);
            await SeedBank("monobank", BankSeedData.MonoUsers);
            await SeedBank("oschad", BankSeedData.OschadUsers);
            await SeedBank("universal", BankSeedData.UniversalUsers);
        }

        private async Task SeedBank(string bank, BankUser[] users)
        {
            var db = _factory.Create(bank);

            await db.Database.EnsureCreatedAsync();

            if (!await db.BankUsers.AnyAsync())
            {
                db.BankUsers.AddRange(users);
                await db.SaveChangesAsync();
                Console.WriteLine($"Seeded users into {bank} database");
            }
            else
            {
                Console.WriteLine($"{bank} already has data — skipping");
            }
        }
    }
}
