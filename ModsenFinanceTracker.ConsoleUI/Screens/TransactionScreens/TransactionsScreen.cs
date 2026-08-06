using MediatR;
using ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.GetTransactionsPaged;
using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.ConsoleUI.Screens.BaseScreens;
using ModsenFinanceTracker.Domain.Configutaion;
using Spectre.Console;

namespace ModsenFinanceTracker.ConsoleUI.Screens.TransactionScreens;

public class TransactionsScreen : BaseLeafScreen
{
    private readonly IMediator _mediator;
    private readonly AppConfiguration _configuration;

    public TransactionsScreen(IMediator mediator, AppConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    protected override string GetHeader() => "[bold green]История транзакций[/]";

    protected override async Task HandleAsync()
    {
        int currentPage = 1;
        const int pageSize = 5; 

        DateTime? startDate = null;
        DateTime? endDate = null;

        if (AnsiConsole.Confirm("Настроить фильтр по датам?", defaultValue: false))
        {
            startDate = PromptDateFilter("Введите начальную дату (дд.мм.гггг):");
            endDate = PromptDateFilter("Введите конечную дату (дд.мм.гггг):");
        }

        bool keepBrowsing = true;

        while (keepBrowsing)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule(GetHeader()));

            if (startDate.HasValue || endDate.HasValue)
            {
                AnsiConsole.MarkupLine($"[grey]Фильтр:[/] {startDate?.ToString("dd.MM.yyyy") ?? "—"} по {endDate?.ToString("dd.MM.yyyy") ?? "—"}");
                AnsiConsole.WriteLine();
            }

            var query = new GetTransactionsPagedQuery(
                startDate,
                endDate,
                currentPage,
                pageSize
            );

            PagedResultDto<TransactionDto> result = await _mediator.Send(query);

            if (result.TotalCount == 0)
            {
                AnsiConsole.MarkupLine("[yellow]Транзакции за выбранный период не найдены.[/]");
                break;
            }

            RenderTable(result);

            var options = new List<string>();

            if (result.HasNextPage)
            {
                options.Add("Следующая страница");
            }

            if (result.HasPreviousPage)
            {
                options.Add("Предыдущая страница");
            }

            options.Add("Выход в меню");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Страница [bold green]{result.PageNumber}[/] из [bold green]{result.TotalPages}[/]:")
                    .AddChoices(options));

            switch (choice)
            {
                case "Следующая страница":
                    currentPage++;
                    break;
                case "Предыдущая страница":
                    currentPage--;
                    break;
                case "Выход в меню":
                    keepBrowsing = false;
                    break;
            }
        }
    }

    private void RenderTable(PagedResultDto<TransactionDto> result)
    {
        var table = new Table()
            .Title($"[bold green]Операции ({result.TotalCount} всего)[/]")
            .AddColumn(new TableColumn("[bold]Дата[/]").Centered())
            .AddColumn(new TableColumn("[bold]Тип[/]"))
            .AddColumn(new TableColumn("[bold]Категория[/]"))
            .AddColumn(new TableColumn("[bold]Описание[/]"))
            .AddColumn(new TableColumn("[bold]Сумма[/]").RightAligned());

        foreach (var item in result.Items)
        {
            var isIncome = item.Type.Equals("Income", StringComparison.OrdinalIgnoreCase);
            var amountColor = isIncome ? "green" : "red";
            var amountSign = isIncome ? "+" : "-";

            table.AddRow(
                item.DateTime.ToString("dd.MM.yyyy HH:mm"),
                isIncome ? "[green]Доход[/]" : "[red]Расход[/]",
                Markup.Escape(item.CategoryName ?? "Без категории"),
                Markup.Escape(item.Description ?? "—"),
                $"[{amountColor}]{amountSign}{item.Amount:F2} {_configuration.Currency}[/]"
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private DateTime? PromptDateFilter(string prompt)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
                .AllowEmpty()
                .PromptStyle("yellow"));

        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        if (DateTime.TryParse(input, out var parsedDate))
        {
            return parsedDate;
        }

        AnsiConsole.MarkupLine("[red]Некорректный формат даты[/]");
        return null;
    }
}