using MediatR;
using ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.CreateTransaction;
using ModsenFinanceTracker.ConsoleUI.Screens.BaseScreens;
using ModsenFinanceTracker.Domain.Configutaion;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;
using ModsenFinanceTracker.Infrastructure.InMemoryStorage;
using Spectre.Console;

namespace ModsenFinanceTracker.ConsoleUI.Screens.TransactionScreens;

public class CreateTransactionScreen : BaseLeafScreen
{
    private readonly IMediator _mediator;
    private readonly InMemoryDbContext _dbContext;
    private readonly AppConfiguration _configuration;

    public CreateTransactionScreen(IMediator mediator, InMemoryDbContext dbContext, AppConfiguration configuration)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    protected override string GetHeader() => "[bold green]Добавление новой транзакции[/]";

    protected override async Task HandleAsync()
    {
        var wallet = _dbContext.Wallets.FirstOrDefault();
        if (wallet == null)
        {
            AnsiConsole.MarkupLine("[red]Ошибка:[/] Кошелек не найден.");
            return;
        }

        var transactionType = AnsiConsole.Prompt(
            new SelectionPrompt<TransactionType>()
                .Title("Выберите тип операции:")
                .UseConverter(t => t == TransactionType.Income ? "[green]Доход[/]" : "[red]Расход[/]")
                .AddChoices(TransactionType.Income, TransactionType.Expense));

        var amount = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Введите сумму:")
                .Validate(val => val > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Сумма должна быть больше нуля![/]")));

        var categories = _dbContext.Categories.ToList();

        var selectedCategory = AnsiConsole.Prompt(
            new SelectionPrompt<Category>()
                .Title("Выберите категорию:")
                .UseConverter(c => c.Name)
                .AddChoices(categories));

        var description = AnsiConsole.Prompt(
            new TextPrompt<string>("Введите описание:")
                .AllowEmpty());

        var command = new CreateTransactionCommand(
            transactionType,
            amount,
            wallet.Id,
            selectedCategory.Id,
            description,
            DateTime.UtcNow
        );

        var newTransactionId = await _mediator.Send(command);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"Транзакция на сумму [yellow]{amount:F2} {_configuration.Currency}[/] создана");
    }
}