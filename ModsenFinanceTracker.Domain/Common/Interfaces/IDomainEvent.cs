using MediatR;

namespace ModsenFinanceTracker.Domain.Common.Interfaces;

public interface IDomainEvent : INotification
{
    DateTime Timestamp { get; }
}
