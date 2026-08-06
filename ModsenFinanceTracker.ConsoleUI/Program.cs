using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModsenFinanceTracker.Application;
using ModsenFinanceTracker.ConsoleUI;
using ModsenFinanceTracker.ConsoleUI.Screens;
using ModsenFinanceTracker.Domain.Configutaion;
using ModsenFinanceTracker.Infrastructure;
using ModsenFinanceTracker.Infrastructure.InMemoryStorage;

var services = new ServiceCollection();


services.AddLogging(builder =>
{
    builder.ClearProviders();
});

services.AddSingleton(sp => new AppConfiguration("BYN", "data.json", "dd.MM.yyyy"));

services.AddApplication();

services.AddInfrastructure();

services.AddScreens();
var serviceProvider = services.BuildServiceProvider();

var dbContext = serviceProvider.GetRequiredService<InMemoryDbContext>();
InMemoryDbContextSeeder.Seed(dbContext);



var mainMenu = serviceProvider.GetRequiredService<MainMenuScreen>();
await mainMenu.ShowAsync();