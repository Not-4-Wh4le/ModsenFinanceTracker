namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Models;

public class TransactionDataModel
{
    public Guid Id { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid WalletId { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
}