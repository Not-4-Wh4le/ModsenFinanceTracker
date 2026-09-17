namespace ModsenFinanceTracker.TelegramBot.States;

public enum AddTransactionStep
{
    None,
    AwaitingWallet,
    AwaitingType,
    AwaitingAmount,
    AwaitingCategory,
    AwaitingDescription
}
