using ModsenFinanceTracker.Domain.Configutaion;
using ModsenFinanceTracker.TelegramBot.Endpoints;
using ModsenFinanceTracker.TelegramBot.States;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ModsenFinanceTracker.TelegramBot;

public static class DependencyInjection
{
    private const string TelegramTokenKey = "TelegramBot:Token";
    private const string MissingTokenErrorMessage = "TelegramBot:Token not specified in configuration";
    
    public static IServiceCollection AddTelegramBot(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        string botToken = configuration[TelegramTokenKey]
            ?? throw new InvalidOperationException(MissingTokenErrorMessage);

        services.AddSingleton<ITelegramBotClient>(
            sp => new TelegramBotClient(botToken));

        var appConfig = configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>()
            ?? new AppConfiguration(
                    AppConfiguration.DefaultCurrency, 
                    AppConfiguration.DefaultDataFilePath, 
                    AppConfiguration.DefaultDateFormat); 

        services.AddSingleton(appConfig);

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
