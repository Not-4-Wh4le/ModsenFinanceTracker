using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Repositories;

public class JsonTransactionRepository : ITransactionRepository
{
    private readonly JsonDbContext _context;

    public JsonTransactionRepository(JsonDbContext context)
    {
        _context = context;
    }

    public Task<(IReadOnlyCollection<Transaction>, int TotalCount)> GetPagedAsync(
        Guid? walletId,
        DateTime? startDate,
        DateTime? endDate,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Transaction> query;

        if (walletId.HasValue && walletId.Value != Guid.Empty)
        {
            var wallet = _context.Wallets.FirstOrDefault(w => w.Id == walletId.Value);
            query = wallet?.Transactions ?? Enumerable.Empty<Transaction>();
        }
        else
        {
            query = _context.Wallets.SelectMany(w => w.Transactions);
        }

        if (startDate.HasValue)
        {
            query = query.Where(t => t.DateTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.DateTime <= endDate.Value);
        }

        query = query.OrderByDescending(t => t.DateTime);

        var totalCount = query.Count();

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult<(IReadOnlyCollection<Transaction>, int)>((items, totalCount));
    }
}
