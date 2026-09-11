using ModsenFinanceTracker.Application;
using ModsenFinanceTracker.Infrastructure;
using ModsenFinanceTracker.Infrastructure.JsonStorage;
using ModsenFinanceTracker.TelegramBot;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddTelegramBot(builder.Configuration);
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
