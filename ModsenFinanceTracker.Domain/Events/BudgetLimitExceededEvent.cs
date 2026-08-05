using ModsenFinanceTracker.Domain.Common.Interfaces;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Domain.Events;

public record BudgetLimitExceededEvent(
    Category Category,
    decimal CurrentExpenses,
    decimal BudgetLimit,
    DateTime Timestamp) : IDomainEvent;
