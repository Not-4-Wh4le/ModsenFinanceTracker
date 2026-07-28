using ModsenFinanceTracker.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Entities
{
    public abstract class Transaction : IEntity
    {
        public Guid Id { get; init; }
        public decimal Amount { get; init; }
        public DateTime DateTime { get; init; }
        public Category Category { get; init; }
        public string Description { get; private set; }

        protected Transaction(
            Guid id, 
            decimal amount,  
            Category category, 
            string desctiption, 
            DateTime? dateTime = null)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Transaction amount must be non-negative");
            }
            
            Id = id;
            Amount = amount;
            Category = category
                ?? throw new ArgumentNullException("Category cannot be null");
            DateTime = dateTime ?? DateTime.UtcNow;
            SetDescription(desctiption);
           
        }

        public abstract void Apply(Wallet wallet);

        public void SetDescription(string description)
        {
            description ??= string.Empty;
            
            if(description.Length > 200)
            {
                throw new ArgumentException("Description length cannot be longer than 200 characters");
            }
            
            Description = description;
        }

    }
}
