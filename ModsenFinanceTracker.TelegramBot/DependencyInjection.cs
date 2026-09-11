using ModsenFinanceTracker.Domain.Configutaion;
using ModsenFinanceTracker.TelegramBot.Commands;
using ModsenFinanceTracker.TelegramBot.States;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ModsenFinanceTracker.TelegramBot;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        string botToken = configuration["TelegramBot:Token"]
            ?? throw new InvalidOperationException("TelegramBot:Token not specified");

        services.AddSingleton<ITelegramBotClient>(
            sp => new TelegramBotClient(botToken));

        services.AddSingleton(sp => new AppConfiguration("BYN", "data.json", "dd.MM.yyyy"));

        var handlerType = typeof(IBotEndpoint);
        var handlers = typeof(Program).Assembly
            .GetTypes()
            .Where(t => handlerType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var handler in handlers)
        {
            services.AddTransient(handlerType, handler);
        }

        services.AddSingleton<TransactionDraft>();
        services.AddSingleton<IUpdateHandler, UpdateHandler>();

        return services;
    }
}
