using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<(IReadOnlyCollection<Category>, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task SaveAsync(Category category, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
