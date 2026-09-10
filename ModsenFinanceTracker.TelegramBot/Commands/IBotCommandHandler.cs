using Telegram.Bot;
using Telegram.Bot.Types;

namespace ModsenFinanceTracker.TelegramBot.Commands;

public interface IBotCommandHandler
{
    string CommandName { get; }

    Task HandleAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken);
}
