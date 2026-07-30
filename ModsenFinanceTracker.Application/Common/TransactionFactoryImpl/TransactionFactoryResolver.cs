using ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;
using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.TransactionFactoryImpl;

public class TransactionFactoryResolver : ITransactionFactoryResolver
{
    private readonly IReadOnlyDictionary<TransactionType, ITransactionFactory> _transactionFactories;

    public TransactionFactoryResolver(IEnumerable<ITransactionFactory> transactionFactories)
    {
        _transactionFactories = transactionFactories.ToDictionary(f => f.Type);
    }

    public ITransactionFactory GetFactory(TransactionType transactionType)
    {
        return _transactionFactories[transactionType];
    }
}
