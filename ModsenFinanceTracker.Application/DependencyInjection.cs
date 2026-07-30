using Microsoft.Extensions.DependencyInjection;
using ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;
using ModsenFinanceTracker.Application.Common.TransactionFactoryImpl;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModsenFinanceTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddSingleton<ITransactionFactory, IncomeTransactionFactory>();
        services.AddSingleton<ITransactionFactory, ExpenseTransactionFactory>();
        services.AddSingleton<ITransactionFactoryResolver, TransactionFactoryResolver>();

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(assembly);
        });

        return services;
    }
}
