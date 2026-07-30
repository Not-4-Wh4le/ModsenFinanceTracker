using ModsenFinanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
