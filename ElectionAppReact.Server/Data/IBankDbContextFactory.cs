using ElectionAppReact.Server.Data;

namespace ElectionAppReact.Server.Data
{
    public interface IBankDbContextFactory
    {
        BankDbContext Create(string bank);
    }
}
