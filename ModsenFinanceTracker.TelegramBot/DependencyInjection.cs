using Microsoft.Extensions.Options;
using ModsenFinanceTracker.Application.Common.Interfaces.Services;
using ModsenFinanceTracker.Domain.Configutaion;
using ModsenFinanceTracker.TelegramBot.Endpoints;
using ModsenFinanceTracker.TelegramBot.Notification;
using ModsenFinanceTracker.TelegramBot.Options;
using ModsenFinanceTracker.TelegramBot.States;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ModsenFinanceTracker.TelegramBot;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBot(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var appConfig = configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>()
            ?? new AppConfiguration(
                    AppConfiguration.DefaultCurrency, 
                    AppConfiguration.DefaultDataFilePath, 
                    AppConfiguration.DefaultDateFormat);

        services.Configure<TelegramBotOptions>(configuration.GetSection(TelegramBotOptions.SectionName));

        services.AddSingleton(appConfig);

        services.AddSingleton<ITelegramBotClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
            return new TelegramBotClient(options.Token);
        });

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
        services.AddTransient<INotificationChannel, TelegramNotificationChannel>();

        return services;
    }
}
