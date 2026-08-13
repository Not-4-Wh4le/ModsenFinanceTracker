using MediatR;
using ModsenFinanceTracker.Application.Common.Models;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.GetTransactionsPaged;

public record GetTransactionsPagedQuery(
    Guid? WalletId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int PageNumber = 1,
    int PageSize = 10)
    : IRequest<PagedResultDto<TransactionDto>>;
