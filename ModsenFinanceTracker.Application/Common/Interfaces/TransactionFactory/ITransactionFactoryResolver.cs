using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Interfaces.TransactionFactory;

public interface ITransactionFactoryResolver
{
    ITransactionFactory GetFactory(TransactionType transactionType);
}
