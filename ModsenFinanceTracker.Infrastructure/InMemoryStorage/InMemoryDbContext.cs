using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Infrastructure.InMemoryStorage;

public class InMemoryDbContext
{
    public List<Wallet> Wallets { get; } = new();
    public List<Category> Categories { get; } = new();
}
