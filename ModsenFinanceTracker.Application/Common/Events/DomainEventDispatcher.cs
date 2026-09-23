using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Events;
using ModsenFinanceTracker.Domain.Common;

namespace ModsenFinanceTracker.Application.Common.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;

    public DomainEventDispatcher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task DispatchAndClearAsync(AggregateRoot root, CancellationToken cancellationToken = default)
    {
        var events = root.Events;
        
        foreach(var e in events)
        {
            await _publisher.Publish(e, cancellationToken); 
        }

        root.ClearDomainEvents();
    }
}
