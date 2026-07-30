using ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.TransactionFactoryImpl;

public class IncomeTransactionFactory : ITransactionFactory
{
    public TransactionType Type => TransactionType.Income;

    public Transaction Create(Guid id, decimal amount, Category category, string description, DateTime? dateTime = null)
    {
        return new IncomeTransaction(
            id,
            amount,
            category,
            description,
            dateTime);
    }
}
