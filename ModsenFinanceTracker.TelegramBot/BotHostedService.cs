using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ModsenFinanceTracker.TelegramBot;

public class BotHostedService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _updateHandler;
    private readonly ILogger<BotHostedService> _logger;

    public BotHostedService(
        ITelegramBotClient botClient,
        IUpdateHandler updateHandler,
        ILogger<BotHostedService> logger)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
            DropPendingUpdates = true 
        };

        User me = await _botClient.GetMe(stoppingToken);
        _logger.LogInformation("{FirstName} (@{Username}) running", me.FirstName, me.Username);

        await _botClient.ReceiveAsync(
            updateHandler: _updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken
        );
    }
}