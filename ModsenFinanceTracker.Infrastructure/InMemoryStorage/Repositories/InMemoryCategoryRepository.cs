using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Infrastructure.InMemoryStorage.Repositories;

public class InMemoryCategoryRepository : ICategoryRepository
{

    private readonly InMemoryDbContext _context;

    public InMemoryCategoryRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _context.Categories.RemoveAll(c => c.Id == id);
        
        return Task.CompletedTask;
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);

        return Task.FromResult(category);
    }

    public Task<(IReadOnlyCollection<Category>, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var categories = _context.Categories
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalCount = _context.Categories.Count;
        
        return Task.FromResult(((IReadOnlyCollection<Category>)categories, totalCount));
    }

    public Task SaveAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (!_context.Categories.Any(c => c.Id == category.Id))
        {
            _context.Categories.Add(category);
        }

        return Task.CompletedTask;
    }
}
