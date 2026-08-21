namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Models;

public record CategoryDataModel(
    Guid Id,
    string Name,
    string TransactionType,
    decimal? BudgetLimit);
