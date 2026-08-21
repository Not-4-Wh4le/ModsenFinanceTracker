namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Models;

public record DataSnapshot(
    List<WalletDataModel> Wallets,
    List<CategoryDataModel> Categories,
    List<TransactionDataModel> Transactions);