using ModsenFinanceTracker.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Common
{
    public abstract class AggregateRoot
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
}
