namespace ModsenFinanceTracker.Application.Common.Interfaces.Services;

public interface INotificationChannel
{
    Task SendAsync(string title, string message, CancellationToken cancellationToken = default);
}
