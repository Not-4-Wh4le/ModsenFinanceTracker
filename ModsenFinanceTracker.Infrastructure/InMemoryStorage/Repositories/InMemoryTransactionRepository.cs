using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Infrastructure.InMemoryStorage.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly InMemoryDbContext _context;

    public InMemoryTransactionRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public Task<(IReadOnlyCollection<Transaction>, int TotalCount)> GetPagedAsync(
        DateTime? startDate, 
        DateTime? endDate, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var transactions = _context.Wallets.SelectMany(w => w.Transactions);

        if (startDate.HasValue)
        {
            transactions = transactions.Where(t => t.DateTime >= startDate);
        }

        if (endDate.HasValue)
        {
            transactions = transactions.Where(t => t.DateTime <= endDate);
        }

        transactions = transactions.OrderByDescending(t => t.DateTime);

        var totalCount = transactions.Count();

        var items = transactions
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(((IReadOnlyCollection<Transaction>)items, totalCount));
    }
}
