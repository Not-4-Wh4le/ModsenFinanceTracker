using ModsenFinanceTracker.Domain.Common;
using ModsenFinanceTracker.Domain.Enums;
using ModsenFinanceTracker.Domain.Events;
using ModsenFinanceTracker.Domain.Exceptions;

namespace ModsenFinanceTracker.Domain.Entities;

public class Wallet : AggregateRoot
{
    private List<Transaction> _transactions = new();

    public decimal Balance => _transactions.Sum(t => t.Contribution());
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public void AddTransaction(Transaction transaction)
    {
        if (Balance + transaction.Contribution() < 0)
        {
            throw new InsufficientFundsException(Balance, transaction.Amount);
        }

        _transactions.Add(transaction);
        CheckCategoryBudgetLimit(transaction);
    }

    public void RemoveTransaction(Guid id)
    {
        var transaction = _transactions.FirstOrDefault(t => t.Id == id);

        if (transaction == null)
        {
            return;
        }

        _transactions.Remove(transaction);
    }

    public void UpdateTransactionDescription(Guid id, string newDescription)
    {
        var transaction = _transactions.FirstOrDefault(t => t.Id == id);

        if (transaction == null)
        {
            return;
        }

        transaction.Description = newDescription;
    }

    private void CheckCategoryBudgetLimit(Transaction transaction)
    {
        var category = transaction.Category;

        if (category.TransactionType != TransactionType.Expense || !category.BudgetLimit.HasValue)
        {
            return;
        }

        decimal currentMonthExpenses = _transactions
            .Where(t => t.Category.Id == category.Id
                && t.DateTime.Year == transaction.DateTime.Year
                && t.DateTime.Month == transaction.DateTime.Month)
            .Sum(t => t.Amount);

        if(currentMonthExpenses > category.BudgetLimit)
        {
            AddDomainEvent(
                new BudgetLimitExceededEvent(
                    category, 
                    currentMonthExpenses, 
                    category.BudgetLimit.Value, 
                    DateTime.UtcNow));
        }
    }
}
