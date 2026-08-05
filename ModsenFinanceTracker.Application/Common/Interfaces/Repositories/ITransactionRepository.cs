using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<(IReadOnlyCollection<Transaction>, int TotalCount)> GetPagedAsync(
            DateTime? startDate,
            DateTime? endDate,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
