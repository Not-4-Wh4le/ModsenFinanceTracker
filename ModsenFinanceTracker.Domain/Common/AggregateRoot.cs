using ModsenFinanceTracker.Domain.Common.Interfaces;

namespace ModsenFinanceTracker.Domain.Common;

public abstract class AggregateRoot : Entity
{
    private List<IDomainEvent> _events = new(); 

    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();
    
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }

    public void ClearDomainEvents()
        => _events.Clear();
}