using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Infrastructure.InMemoryStorage.Repositories;

public class InMemoryWalletRepository : IWalletRepository
{
    private readonly InMemoryDbContext _context;

    public InMemoryWalletRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public Task<Wallet?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var wallet = _context.Wallets.FirstOrDefault(w => w.Id == Id);
        return Task.FromResult(wallet);
    }

    public Task SaveAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        if (!_context.Wallets.Any(w => w.Id == wallet.Id))
        {
            _context.Wallets.Add(wallet);
        }

        return Task.CompletedTask;
    }
}
