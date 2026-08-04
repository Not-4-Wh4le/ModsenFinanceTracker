using ModsenFinanceTracker.Domain.Enums;
using ModsenFinanceTracker.Domain.Exceptions;

namespace ModsenFinanceTracker.Domain.Entities;

public class ExpenseTransaction : Transaction
{
    public ExpenseTransaction(
        Guid id,
        decimal amount,
        Category category,
        string description,
        DateTime? dateTime = null)
        : base(id, amount, category, description, dateTime)
    {
        if(category.TransactionType != TransactionType.Expense)
        {
            throw new CategoryMismatchException(TransactionType.Expense, category.TransactionType);
        }
    }

    public override decimal Contribution()
    {
        return -Amount;
    }
}
