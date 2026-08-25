using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Repositories;

public class JsonTransactionRepository : ITransactionRepository
{
    private readonly JsonDbContext _context;

    public JsonTransactionRepository(JsonDbContext context)
    {
        _context = context;
    }

    public Task<PagedResult<Transaction>> GetPagedAsync(
        Guid? walletId,
        TransactionType? transactionType,
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

        if (transactionType.HasValue)
        {
            query = query.Where(t => t.Category.TransactionType == transactionType.Value);
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

        var result = new PagedResult<Transaction>(
            items,
            totalCount,
            pageNumber,
            pageSize);

        return Task.FromResult(result);
    }
}
