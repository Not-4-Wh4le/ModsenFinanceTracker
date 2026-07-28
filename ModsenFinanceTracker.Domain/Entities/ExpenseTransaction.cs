using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Entities
{
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
                throw new ArgumentException("Cannot assign an Income category to an Expense transaction");
            }
        }

        public override void Apply(Wallet wallet)
        {
            wallet.ApplyExpense(this);
        }
    }
}
