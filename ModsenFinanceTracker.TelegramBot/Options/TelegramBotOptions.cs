namespace ModsenFinanceTracker.TelegramBot.Options;

public class TelegramBotOptions()
{
    public const string SectionName = "TelegramBot";

    public string Token { get; init; } = string.Empty;
    public long ChatId { get; init; }
}
