using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Repositories;

public class JsonWalletRepository : IWalletRepository
{
    private readonly JsonDbContext _context;

    public JsonWalletRepository(JsonDbContext context)
    {
        _context = context;
    }

    public Task<Wallet?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var wallet = _context.Wallets.FirstOrDefault(w => w.Id == id);
        return Task.FromResult(wallet);
    }

    public Task<(IReadOnlyCollection<Wallet>, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = _context.Wallets.Count;

        var wallets = _context.Wallets
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult<(IReadOnlyCollection<Wallet>, int)>((wallets, totalCount));
    }

    public async Task SaveAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        var existing = _context.Wallets.FirstOrDefault(w => w.Id == wallet.Id);

        if (existing == null)
        {
            _context.Wallets.Add(wallet);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
