using ModsenFinanceTracker.Domain.Common.Interfaces;
using ModsenFinanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Events
{
    public record BudgetLimitExceededEvent(
        Category Category,
        decimal CurrentExpenses,
        decimal BudgetLimit,
        DateTime Timestamp) : IDomainEvent;
}
