using MediatR;
using ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.ExportTransactions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ModsenFinanceTracker.TelegramBot.Commands;

public abstract class BaseExportCommandHandler : IBotCommandHandler
{
    private const string GeneratingMessageTemplate = "Генерирую {0}-отчет...";
    private const string SuccessCaptionTemplate = "Ваш отчет в формате {0}";
    private const string ErrorMessage = "Произошла ошибка при генерации отчета";

    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public abstract string CommandName { get; }
    protected abstract string Format { get; }

    protected BaseExportCommandHandler(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        long chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id ?? 0;
        if (chatId == 0) return;

        await botClient.SendMessage(
            chatId: chatId,
            text: string.Format(GeneratingMessageTemplate, Format),
            cancellationToken: cancellationToken);

        try
        {
            var exportResult = await _mediator.Send(
                new ExportTransactionsQuery(Format),
                cancellationToken);

            await using (exportResult.FileContents)
            {
                var inputFile = InputFile.FromStream(exportResult.FileContents, exportResult.FileName);

                await botClient.SendDocument(
                    chatId: chatId,
                    document: inputFile,
                    caption: string.Format(SuccessCaptionTemplate, Format),
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при экспорте файла формата {Format}", Format);

            await botClient.SendMessage(
                chatId: chatId,
                text: ErrorMessage,
                cancellationToken: cancellationToken);
        }
    }
}