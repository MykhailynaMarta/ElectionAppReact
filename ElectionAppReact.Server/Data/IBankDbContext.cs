using ElectionAppReact.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectionAppReact.Server.Data
{
    public interface IBankDbContext
    {
        DbSet<BankUser> BankUsers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
