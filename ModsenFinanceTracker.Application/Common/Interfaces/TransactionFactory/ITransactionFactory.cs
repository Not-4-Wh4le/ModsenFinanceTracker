using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;

public interface ITransactionFactory
{
    TransactionType Type { get; }
    Transaction Create(
        Guid id,
        decimal amount,
        Category category,
        string description,
        DateTime? dateTime = null);
}
