using ModsenFinanceTracker.Application;
using ModsenFinanceTracker.Domain.Configutaion;
using ModsenFinanceTracker.Infrastructure;
using ModsenFinanceTracker.Infrastructure.JsonStorage;
using ModsenFinanceTracker.TelegramBot;
using ModsenFinanceTracker.TelegramBot.Commands;
using ModsenFinanceTracker.TelegramBot.States;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = Host.CreateApplicationBuilder(args);

string botToken = builder.Configuration["TelegramBot:Token"]
    ?? throw new InvalidOperationException("TelegramBot:Token not specified");

builder.Services.AddSingleton<ITelegramBotClient>(
    sp => new TelegramBotClient(botToken));

builder.Services.AddSingleton(sp => new AppConfiguration("BYN", "data.json", "dd.MM.yyyy"));

var handlerType = typeof(IBotCommandHandler);
var handlers = typeof(Program).Assembly
    .GetTypes()
    .Where(t => handlerType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

foreach (var handler in handlers)
{
    builder.Services.AddTransient(handlerType, handler);
}

builder.Services.AddSingleton<TransactionDraft>();
builder.Services.AddSingleton<IUpdateHandler, UpdateHandler>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddHostedService<BotHostedService>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<JsonDbContext>();
    await dbContext.LoadAsync();
}

host.Run();
