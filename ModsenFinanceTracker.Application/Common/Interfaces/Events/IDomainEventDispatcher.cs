using ModsenFinanceTracker.Domain.Common;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearAsync(AggregateRoot root, CancellationToken cancellationToken = default);
}
