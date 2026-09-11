using MediatR;
using ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoriesPaged;
using ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.CreateTransaction;
using ModsenFinanceTracker.Application.Common.Features.Wallets.Queries.GetWalletsPaged;
using ModsenFinanceTracker.Domain.Enums;
using ModsenFinanceTracker.TelegramBot.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ModsenFinanceTracker.TelegramBot.Endpoints;

public class AddTransactionEndpoint : IBotEndpoint
{
    private const string CommandNameValue = "/add";
    private const string WalletNotFoundMessage = "Кошельки не найдены";
    private const string SelectWalletMessage = "Выберите кошелек:";
    private const string SelectTypeMessage = "Выберите тип операции:";
    private const string EnterAmountMessage = "Введите сумму:";
    private const string InvalidAmountMessage = "Некорректная сумма. Введите положительное число:";
    private const string CategoriesNotFoundMessage = "Категории не найдены.";
    private const string SelectCategoryMessage = "Выберите категорию:";
    private const string EnterDescriptionMessage = "Введите описание:";
    private const string DefaultDescription = "Транзакция из Telegram";
    private const string SuccessMessage = "Транзакция добавлена";

    private const string WalletCallbackPrefix = "wallet_";
    private const string TypeCallbackPrefix = "type_";
    private const string CategoryCallbackPrefix = "cat_";

    private const string IncomeTypeName = "Income";
    private const string ExpenseTypeName = "Expense";
    private const string IncomeDisplayName = "Доход";
    private const string ExpenseDisplayName = "Расход";

    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 50;

    private readonly IMediator _mediator;
    private readonly TransactionDraft _draft;

    public string CommandName => CommandNameValue;

    public AddTransactionEndpoint(IMediator mediator, TransactionDraft draft)
    {
        _mediator = mediator;
        _draft = draft;
    }

    public async Task HandleAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;

        if (chatId == null)
        {
            return;
        }

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message
            && update.Message!.Text == CommandNameValue)
        {
            _draft.Reset();
            _draft.Step = AddTransactionStep.AwaitingWallet;
            await AskForWalletAsync(botClient, chatId.Value, cancellationToken);

            return;
        }

        await (_draft.Step switch
        {
            AddTransactionStep.AwaitingWallet => ProcessWalletSelectionAsync(botClient, update, cancellationToken),
            AddTransactionStep.AwaitingType => ProcessTypeSelectionAsync(botClient, update, cancellationToken),
            AddTransactionStep.AwaitingAmount => ProcessAmountInputAsync(botClient, update, cancellationToken),
            AddTransactionStep.AwaitingCategory => ProcessCategorySelectionAsync(botClient, update, cancellationToken),
            AddTransactionStep.AwaitingDescription => ProcessDescriptionAndSaveAsync(botClient, update, cancellationToken),
            _ => Task.CompletedTask
        });
    }

    private async Task AskForWalletAsync(
        ITelegramBotClient botClient, 
        long chatId, 
        CancellationToken cancellationToken)
    {
        var wallets = await _mediator.Send(
            new GetWalletsPagedQuery(DefaultPageNumber, DefaultPageSize), 
            cancellationToken);

        if (wallets.Items == null || !wallets.Items.Any())
        {
            await botClient.SendMessage(
                chatId, 
                WalletNotFoundMessage, 
                cancellationToken: cancellationToken);
            _draft.Reset();

            return;
        }

        var buttons = wallets.Items.Select(w =>
            new[] { InlineKeyboardButton.WithCallbackData(w.Name, $"{WalletCallbackPrefix}{w.Id}") });

        await botClient.SendMessage(
            chatId, 
            SelectWalletMessage,
            replyMarkup: new InlineKeyboardMarkup(buttons), 
            cancellationToken: cancellationToken);
    }

    private async Task ProcessWalletSelectionAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        if (update.CallbackQuery == null 
            || !update.CallbackQuery.Data!.StartsWith(WalletCallbackPrefix))
        {
            return;
        }

        _draft.WalletId = Guid.Parse(update.CallbackQuery.Data.Replace(WalletCallbackPrefix, ""));
        _draft.Step = AddTransactionStep.AwaitingType;

        var buttons = new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(IncomeDisplayName, $"{TypeCallbackPrefix}{IncomeTypeName}"),
                InlineKeyboardButton.WithCallbackData(ExpenseDisplayName, $"{TypeCallbackPrefix}{ExpenseTypeName}")
            }
        };

        var message = update.CallbackQuery.Message!;

        await botClient.EditMessageText(
            message.Chat.Id,
            message.MessageId,
            SelectTypeMessage,
            replyMarkup: new InlineKeyboardMarkup(buttons),
            cancellationToken: cancellationToken);
    }

    private async Task ProcessTypeSelectionAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        if (update.CallbackQuery == null 
            || !update.CallbackQuery.Data!.StartsWith(TypeCallbackPrefix))
        {
            return;
        }

        _draft.Type = Enum.Parse<TransactionType>(update.CallbackQuery.Data.Replace(TypeCallbackPrefix, ""));
        _draft.Step = AddTransactionStep.AwaitingAmount;

        var message = update.CallbackQuery.Message!;
        await botClient.SendMessage(
            message.Chat.Id,
            EnterAmountMessage,
            cancellationToken: cancellationToken);
    }

    private async Task ProcessAmountInputAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        var chatId = update.Message!.Chat.Id;

        if (!decimal.TryParse(
                update.Message.Text?.Replace(',', '.'), 
                System.Globalization.CultureInfo.InvariantCulture, 
                out var amount)
            || amount <= 0)
        {
            await botClient.SendMessage(
                chatId, InvalidAmountMessage, 
                cancellationToken: cancellationToken);

            return;
        }

        _draft.Amount = amount;

        var categories = await _mediator.Send(
            new GetCategoriesPagedQuery(_draft.Type, DefaultPageNumber, DefaultPageSize), 
            cancellationToken);

        if (categories.Items == null || !categories.Items.Any())
        {
            await botClient.SendMessage(
                chatId, 
                CategoriesNotFoundMessage, 
                cancellationToken: cancellationToken);

            _draft.Reset();

            return;
        }

        _draft.Step = AddTransactionStep.AwaitingCategory;

        var buttons = categories.Items.Select(c =>
            new[] { InlineKeyboardButton.WithCallbackData(c.Name, $"{CategoryCallbackPrefix}{c.Id}") });

        await botClient.SendMessage(
            chatId, 
            SelectCategoryMessage,
            replyMarkup: new InlineKeyboardMarkup(buttons), cancellationToken: cancellationToken);
    }

    private async Task ProcessCategorySelectionAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        if (update.CallbackQuery == null || !update.CallbackQuery.Data!.StartsWith(CategoryCallbackPrefix))
        {
            return;
        }

        _draft.CategoryId = Guid.Parse(update.CallbackQuery.Data.Replace(CategoryCallbackPrefix, ""));
        _draft.Step = AddTransactionStep.AwaitingDescription;

        await botClient.SendMessage(
            update.CallbackQuery.Message!.Chat.Id,
            EnterDescriptionMessage,
            cancellationToken: cancellationToken);
    }

    private async Task ProcessDescriptionAndSaveAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        var chatId = update.Message!.Chat.Id;
        var descriptionInput = update.Message.Text?.Trim();
        var description = (string.IsNullOrEmpty(descriptionInput) || descriptionInput == "-")
            ? DefaultDescription
            : descriptionInput;

        var command = new CreateTransactionCommand(
            _draft.Type,
            _draft.Amount,
            _draft.WalletId,
            _draft.CategoryId,
            description,
            DateTime.UtcNow);

        var transactionId = await _mediator.Send(command, cancellationToken);

        await botClient.SendMessage(
            chatId,
            SuccessMessage,
            cancellationToken: cancellationToken);

        _draft.Reset();
    }
}