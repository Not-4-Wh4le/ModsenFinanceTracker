namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Models;

public class DataSnapshot
{
    public List<WalletDataModel> Wallets { get; set; } = new();
    public List<CategoryDataModel> Categories { get; set; } = new();
    public List<TransactionDataModel> Transactions { get; set; } = new();
}