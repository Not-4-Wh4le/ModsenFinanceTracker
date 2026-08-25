namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Models;

public record TransactionDataModel(
    Guid Id,
    string TransactionType,
    decimal Amount,
    Guid WalletId,
    Guid CategoryId,
    string Description,
    DateTime DateTime);