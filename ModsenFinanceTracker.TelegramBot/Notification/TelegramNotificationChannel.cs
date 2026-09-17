using Microsoft.Extensions.Options;
using ModsenFinanceTracker.Application.Common.Interfaces.Services;
using ModsenFinanceTracker.TelegramBot.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ModsenFinanceTracker.TelegramBot.Notification;

public class TelegramNotificationChannel : INotificationChannel
{
    private const string NotificationMessage = "<b>{0}</b>\n\n{1}";
    private readonly ITelegramBotClient _botClient;
    private readonly long _chatId;

    public TelegramNotificationChannel(ITelegramBotClient botClient, IOptions<TelegramBotOptions> options)
    {
        _botClient = botClient;
        _chatId = options.Value.ChatId;
    }

    public async Task SendAsync(string title, string message, CancellationToken cancellationToken)
    {
        await _botClient.SendMessage(
            _chatId, 
            string.Format(NotificationMessage, title, message), 
            ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
