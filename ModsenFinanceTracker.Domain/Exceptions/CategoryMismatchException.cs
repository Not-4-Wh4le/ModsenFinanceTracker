using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Domain.Exceptions;

public class CategoryMismatchException : DomainException
{
    public TransactionType ExpectedType { get; }
    public TransactionType ActualType { get; }


    public CategoryMismatchException(TransactionType expedctedType ,TransactionType actualType)
        : base($"Cannot assign a {actualType} category to a {expedctedType} transaction")
    {
        ActualType = actualType;
        ExpectedType = expedctedType;
    }
}
