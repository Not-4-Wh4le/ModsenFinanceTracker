using ModsenFinanceTracker.Domain.Common;
using ModsenFinanceTracker.Domain.Common.Interfaces;
using ModsenFinanceTracker.Domain.Events;
using ModsenFinanceTracker.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Entities
{
    public class Wallet : AggregateRoot
    {
        private List<Transaction> _transactions = new();

        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public void AddTransaction(Transaction transaction)
        {
            transaction.Apply(this);
        }

        internal void ApplyIncome(IncomeTransaction transaction)
        {
            _transactions.Add(transaction);
        }

        internal void ApplyExpense(ExpenseTransaction transaction)
        {
            if(GetBalance() < transaction.Amount)
            {
                throw new InsufficientFundsException(GetBalance(), transaction.Amount);
            }
            _transactions.Add(transaction);
            CheckCategoryBudgetLimit(transaction);

        }

        public decimal GetBalance()
        {
            decimal income = _transactions
                .OfType<IncomeTransaction>()
                .Sum(i => i.Amount);

            decimal expense = _transactions
                .OfType<ExpenseTransaction>()
                .Sum(e => e.Amount);

            return income - expense;
        }

        private void CheckCategoryBudgetLimit(ExpenseTransaction transaction)
        {
            var category = transaction.Category;
            if (!category.BudgetLimit.HasValue)
            {
                return;
            }
            decimal currentMonthExpenses = _transactions
                .OfType<ExpenseTransaction>()
                .Where(e => e.Category.Id == category.Id
                    && e.DateTime.Year == transaction.DateTime.Year
                    && e.DateTime.Month == transaction.DateTime.Month)
                .Sum(e => e.Amount);

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
}
