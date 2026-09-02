using Microsoft.Extensions.DependencyInjection;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Interfaces.Services;
using ModsenFinanceTracker.Infrastructure.JsonStorage;
using ModsenFinanceTracker.Infrastructure.JsonStorage.Repositories;
using ModsenFinanceTracker.Infrastructure.JsonStorage.Services;

namespace ModsenFinanceTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<JsonDbContext>();

        services.AddSingleton<IWalletRepository, JsonWalletRepository>();
        services.AddSingleton<ICategoryRepository, JsonCategoryRepository>();
        services.AddSingleton<ITransactionRepository, JsonTransactionRepository>();
        services.AddSingleton<ICategoryAnalyticsReadService, CategoryAnalyticsReadService>();

        return services;
    }
}
