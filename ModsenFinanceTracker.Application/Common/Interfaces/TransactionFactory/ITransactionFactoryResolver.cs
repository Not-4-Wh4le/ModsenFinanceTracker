using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;

public interface ITransactionFactoryResolver
{
    ITransactionFactory GetFactory(TransactionType transactionType);
}
