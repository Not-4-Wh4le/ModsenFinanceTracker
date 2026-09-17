using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.TelegramBot.States;

public class TransactionDraft
{
    public AddTransactionStep Step { get; set; } = AddTransactionStep.None;
    public Guid WalletId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public Guid CategoryId { get; set; }

    public bool IsActive => Step != AddTransactionStep.None;

    public void Reset()
    {
        Step = AddTransactionStep.None;
        WalletId = Guid.Empty;
        Amount = 0;
        CategoryId = Guid.Empty;
    }
}
