namespace ModsenFinanceTracker.Domain.Common.Interfaces;

public interface IDomainEvent 
{
    DateTime Timestamp { get; }
}
