namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.ExportTransactions;

public record ExportTransactionDto(
    Guid Id,
    string DateTime,
    decimal Amount,
    string Category,
    string Description);