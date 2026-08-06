using Microsoft.Extensions.DependencyInjection;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Infrastructure.InMemoryStorage;
using ModsenFinanceTracker.Infrastructure.InMemoryStorage.Repositories;

namespace ModsenFinanceTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryDbContext>();

        services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();
        services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
        services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();

        return services;
    }
}
