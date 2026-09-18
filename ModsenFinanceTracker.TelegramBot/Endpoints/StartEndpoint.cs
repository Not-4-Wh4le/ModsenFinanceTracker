using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ModsenFinanceTracker.TelegramBot.Endpoints;

public class StartEndpoint : IBotEndpoint
{
    private const string MessageText = "Ваш Chat ID: <code>{0}</code>\n\n" +
                                       "Скопируйте его и укажите в <code>appsettings.json</code>:\n" +
                                       "<code>\"TelegramBot\": {{ \"ChatId\": {0} }}</code>";
    public string CommandName => "/start";

    public async Task HandleAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;

        if(chatId == null)
        {
            return;
        }

        await botClient.SendMessage(
            chatId.Value,
            string.Format(MessageText, chatId.Value),
            ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
