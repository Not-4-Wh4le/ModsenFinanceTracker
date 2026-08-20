using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Repositories;

public class JsonCategoryRepository : ICategoryRepository
{
    private readonly JsonDbContext _context;

    public JsonCategoryRepository(JsonDbContext context)
    {
        _context = context;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category != null)
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(category);
    }

    public Task<PagedResult<Category>> GetPagedAsync(
        TransactionType? transactionType,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Categories.AsEnumerable();

        if (transactionType.HasValue)
        {
            query = query.Where(c => c.TransactionType == transactionType.Value);
        }

        var totalCount = query.Count();

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PagedResult<Category>(
            items,
            totalCount,
            pageNumber,
            pageSize);

        return Task.FromResult(result);
    }

    public async Task SaveAsync(Category category, CancellationToken cancellationToken = default)
    {
        var existing = _context.Categories.FirstOrDefault(c => c.Id == category.Id);

        if (existing == null)
        {
            _context.Categories.Add(category);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}