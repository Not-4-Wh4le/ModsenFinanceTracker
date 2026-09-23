using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Services;
using ModsenFinanceTracker.Domain.Events;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.EventHandlers;

public class BudgetLimitExceededEventHandler
    : INotificationHandler<BudgetLimitExceededEvent>
{
    private const string BudgetLimitExceededTitle = "Превышен лимит бюджета";
    private const string BudgetLimitExceededMessage = "По категории '{0}' потрачено {1} из {2}";

    private readonly IEnumerable<INotificationChannel> _channels;

    public BudgetLimitExceededEventHandler(IEnumerable<INotificationChannel> channels)
    {
        _channels = channels;
    }

    public async Task Handle(BudgetLimitExceededEvent notification, CancellationToken cancellationToken)
    {
        var tasks = _channels.Select(c => 
            c.SendAsync(
                BudgetLimitExceededTitle,
                string.Format(
                    BudgetLimitExceededMessage, 
                    notification.Category.Name, 
                    notification.CurrentExpenses, 
                    notification.BudgetLimit)));
        
        await Task.WhenAll(tasks);
    }
}
