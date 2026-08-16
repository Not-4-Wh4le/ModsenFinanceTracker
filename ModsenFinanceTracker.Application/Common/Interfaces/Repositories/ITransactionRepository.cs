using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<PagedResult<Transaction>> GetPagedAsync(
            Guid? walletId,
            TransactionType? transactionType,
            DateTime? startDate,
            DateTime? endDate,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
