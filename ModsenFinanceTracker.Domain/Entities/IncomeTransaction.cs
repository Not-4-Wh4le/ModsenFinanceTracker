using ModsenFinanceTracker.Domain.Enums;
using ModsenFinanceTracker.Domain.Exceptions;

namespace ModsenFinanceTracker.Domain.Entities;

public class IncomeTransaction : Transaction
{
    public IncomeTransaction(
        Guid id,
        decimal amount,
        Category category,
        string description,
        DateTime? dateTime = null)
        : base(id, amount, category, description, dateTime)
    {
        if (category.TransactionType != TransactionType.Income)
        {
            throw new CategoryMismatchException(TransactionType.Income, category.TransactionType);
        }
    }

    public override decimal Contribution()
    {
        return Amount;
    }
}
