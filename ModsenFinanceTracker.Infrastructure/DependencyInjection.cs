using Microsoft.Extensions.DependencyInjection;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Infrastructure.JsonStorage;
using ModsenFinanceTracker.Infrastructure.JsonStorage.Repositories;

namespace ModsenFinanceTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<JsonDbContext>();

        services.AddSingleton<IWalletRepository, JsonWalletRepository>();
        services.AddSingleton<ICategoryRepository, JsonCategoryRepository>();
        services.AddSingleton<ITransactionRepository, JsonTransactionRepository>();

        return services;
    }
}
