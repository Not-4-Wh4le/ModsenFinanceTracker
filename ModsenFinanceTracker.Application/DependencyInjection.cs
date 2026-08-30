<<<<<<< HEAD
﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModsenFinanceTracker.Application.Common.Behavior;
=======
﻿using Microsoft.Extensions.DependencyInjection;
using ModsenFinanceTracker.Application.Common.Export;
using ModsenFinanceTracker.Application.Common.Interfaces.ReportExportStrategy;
>>>>>>> 3170751 (feat(export): register factory and strategy in di)
using ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;
using ModsenFinanceTracker.Application.Common.TransactionFactoryImpl;
using System.Reflection;

namespace ModsenFinanceTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddSingleton<ITransactionFactory, IncomeTransactionFactory>();
        services.AddSingleton<ITransactionFactory, ExpenseTransactionFactory>();
        services.AddSingleton<ITransactionFactoryResolver, TransactionFactoryResolver>();

        services.AddSingleton<IReportExportStrategy, CsvReportExportStrategy>();
        services.AddSingleton<IReportExportStrategy, TxtReportExportStrategy>();
        services.AddSingleton<IReportExportFactory, ReportExportFactory>();

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(assembly);
            conf.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        });

        return services;
    }
}
