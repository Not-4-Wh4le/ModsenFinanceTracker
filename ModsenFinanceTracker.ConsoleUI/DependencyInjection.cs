using Microsoft.Extensions.DependencyInjection;
using ModsenFinanceTracker.ConsoleUI.Screens;
using ModsenFinanceTracker.ConsoleUI.Screens.TransactionScreens;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.ConsoleUI;
public static class DependencyInjection
{
    public static IServiceCollection AddScreens(this IServiceCollection services)
    {
        services.AddTransient<MainMenuScreen>();
        services.AddTransient<TransactionsScreen>();
        services.AddTransient<CreateTransactionScreen>();

        return services;
    }
}
