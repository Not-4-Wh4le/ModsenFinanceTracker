using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<PagedResult<Category>> GetPagedAsync(
            TransactionType? transactionType,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task SaveAsync(Category category, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
