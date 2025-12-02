using Microsoft.EntityFrameworkCore;
using ElectionAppReact.Server.Data;

public class BankDbContextFactory : IBankDbContextFactory
{
    private readonly IConfiguration _config;

    public BankDbContextFactory(IConfiguration config)
    {
        _config = config;
    }

    public BankDbContext Create(string bank)
    {
        var options = new DbContextOptionsBuilder<BankDbContext>();
        var b = bank.ToLower();

        var runMode = _config["RunMode"]?.ToLower();

        // ---------- LINUX MODE ----------
        if (runMode == "linux")
        {
            // на Linux все працює тільки через PostgreSQL
            options.UseNpgsql(_config.GetConnectionString("MonobankPostgres"));

            var ctx = new BankDbContext(options.Options);
            ctx.Database.EnsureCreated();
            return ctx;
        }

        // ---------- WINDOWS MODE ----------
        if (b.Contains("admin") || b.Contains("mono"))
            options.UseNpgsql(_config.GetConnectionString("MonobankPostgres"));
        else if (b.Contains("privat"))
            options.UseSqlServer(_config.GetConnectionString("PrivatBankSqlServer"));
        else if (b.Contains("oschad"))
            options.UseSqlite("Data Source=oschad.sqlite");
        else
            options.UseInMemoryDatabase("UniversalInMemory");

        var context = new BankDbContext(options.Options);
        context.Database.EnsureCreated();

        return context;
    }
}
