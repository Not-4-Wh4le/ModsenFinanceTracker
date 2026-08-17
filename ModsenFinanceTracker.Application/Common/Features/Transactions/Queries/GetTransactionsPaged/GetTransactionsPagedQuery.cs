using MediatR;
using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.GetTransactionsPaged;

public record GetTransactionsPagedQuery(
    Guid? WalletId = null,
    TransactionType? TransactionType = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int PageNumber = 1,
    int PageSize = 10)
    : IRequest<PagedResult<TransactionDto>>;
