using Spectre.Console;

namespace ModsenFinanceTracker.ConsoleUI.Screens.BaseScreens;

public abstract class BaseMenuScreen
{
    protected abstract string GetHeader();
    protected abstract List<(string Title, Func<Task> Action)> ConfMenu();
    protected virtual string ExitOptionText => "Назад";
    protected virtual bool ShouldClose() => false;
    protected virtual Task OnExitAsync() => Task.CompletedTask;

    public virtual async Task ShowAsync()
    {
        bool isInside = true;

        while (isInside)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule(GetHeader()));

            var menuItems = ConfMenu();

            var selectionPrompt = new SelectionPrompt<string>()
                .AddChoices(menuItems.Select(i => i.Title));

            selectionPrompt.AddChoice(ExitOptionText);

            var choice = AnsiConsole.Prompt(selectionPrompt);

            if (choice == ExitOptionText)
            {
                isInside = false;
                await OnExitAsync();
                continue;
            }

            var selectedAction = menuItems.First(i => i.Title == choice).Action;
            await selectedAction();

            if (ShouldClose())
            {
                isInside = false;
            }
        }
    }
}