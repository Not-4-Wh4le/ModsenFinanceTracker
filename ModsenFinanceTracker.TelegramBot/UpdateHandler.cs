using ModsenFinanceTracker.TelegramBot.Commands;
using ModsenFinanceTracker.TelegramBot.States;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ModsenFinanceTracker.TelegramBot;

public class UpdateHandler : IUpdateHandler
{
    private const string UnknownCommandMessage = "Неизвестная команда.";
    private const string ApiErrorLogMessage = "Telegram API error";
    private readonly Dictionary<string, IBotEndpoint> _handlers;
    private readonly TransactionDraft _draft;
    private readonly ILogger<UpdateHandler> _logger;

    public UpdateHandler(
        IEnumerable<IBotEndpoint> handlers,
        TransactionDraft draft,
        ILogger<UpdateHandler> logger)
    {
        _handlers = handlers.ToDictionary(h => h.CommandName.ToLower(), h => h);
        _draft = draft;
        _logger = logger;
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        long chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id ?? 0;
        if (chatId == 0)
        {
            return;
        }

        var messageText = update.Message?.Text?.Trim();

        if (messageText == "/cancel")
        {
            _draft.Reset();
            await botClient.SendMessage(
                chatId: chatId,
                text: "Операция отменена",
                cancellationToken: cancellationToken);

            return;
        }

        if (_draft.IsActive)
        {
            if (_handlers.TryGetValue("/add", out var addHandler))
            {
                await addHandler.HandleAsync(botClient, update, cancellationToken);

                return;
            }
        }

        if (!string.IsNullOrEmpty(messageText) && messageText.StartsWith('/'))
        {
            var commandName = messageText.Split(' ')[0].ToLower();

            if (_handlers.TryGetValue(commandName, out var handler))
            {
                await handler.HandleAsync(botClient, update, cancellationToken);
            }
            else
            {
                await botClient.SendMessage(
                    chatId,
                    UnknownCommandMessage,
                    cancellationToken: cancellationToken);
            }
        }
    }

    public Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, ApiErrorLogMessage);
        return Task.CompletedTask;
    }
}