using ModsenFinanceTracker.Domain.Common.Interfaces;
using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Entities
{
    public class Category : IEntity
    {
        public Guid Id { get; init; }
        public string Name { get; private set; }
        public TransactionType TransactionType { get; init; }
        public decimal? BudgetLimit { get; private set; }

        public Category(Guid id, string name, TransactionType transactionType, decimal budgetLimit)
        {
            Id = id;
            TransactionType = transactionType;
            SetName(name);
            SetBudgetLimit(budgetLimit);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty");
            }

            if (name.Length > 50)
            {
                throw new ArgumentException("Name length cannot be longer than 50 characters");
            }

            Name = name;
        }

        public void SetBudgetLimit(decimal? budgetLimit)
        {
            if(budgetLimit.HasValue && budgetLimit < 0)
            {
                throw new ArgumentException("Budget limit must be non-negative");
            }

            if(TransactionType == TransactionType.Income && budgetLimit.HasValue)
            {
                throw new InvalidOperationException("Cannot set a budget limit for an income category");
            }

            BudgetLimit = budgetLimit;
        }
    }
}
