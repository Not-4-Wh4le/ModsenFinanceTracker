using MediatR;
using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.ExportTransactions;

public record ExportTransactionsQuery(
    string Format,
     Guid? WalletId = null,
    TransactionType? TransactionType = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null) 
    : IRequest<ExportFileResult>;
