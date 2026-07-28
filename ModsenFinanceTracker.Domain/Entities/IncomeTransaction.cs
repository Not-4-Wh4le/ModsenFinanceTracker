using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Entities
{
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
                throw new ArgumentException("Cannot assign an Expense category to an Income transaction");
            }
        }

        public override void Apply(Wallet wallet)
        {
            wallet.ApplyIncome(this);
        }
    }
}
