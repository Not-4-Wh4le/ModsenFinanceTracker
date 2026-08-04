namespace ModsenFinanceTracker.Domain.Exceptions;

public class InvalidCategoryBudgetLimitException : DomainException
{
    public decimal AttemptedLimit;

    public InvalidCategoryBudgetLimitException(decimal attemptedLimit)
        : base($"Budget limit must be non-negative. Received: {attemptedLimit}")
    {
        AttemptedLimit = attemptedLimit;
    }
}
