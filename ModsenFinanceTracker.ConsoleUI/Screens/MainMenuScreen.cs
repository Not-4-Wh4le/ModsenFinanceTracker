using ModsenFinanceTracker.ConsoleUI.Screens.BaseScreens;
using ModsenFinanceTracker.ConsoleUI.Screens.TransactionScreens;

namespace ModsenFinanceTracker.ConsoleUI.Screens;

public class MainMenuScreen : BaseMenuScreen
{
    private readonly CreateTransactionScreen _createTransaction;
    private readonly TransactionsScreen _transactionsScreen;

    public MainMenuScreen(CreateTransactionScreen createTransaction, TransactionsScreen transactionsScreen)
    {
        _createTransaction = createTransaction;
        _transactionsScreen = transactionsScreen;
    }

    protected override string ExitOptionText => "Выйти";

    protected override List<(string Title, Func<Task> Action)> ConfMenu() => new()
    {
        ("1. Посмотреть транзакции", () => _transactionsScreen.ShowAsync()),
        ("2. Добавить транзакцию", () => _createTransaction.ShowAsync()),
    };

    protected override string GetHeader() => "[bold green]Главное меню[/]";
}
