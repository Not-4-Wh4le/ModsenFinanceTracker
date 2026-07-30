using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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
