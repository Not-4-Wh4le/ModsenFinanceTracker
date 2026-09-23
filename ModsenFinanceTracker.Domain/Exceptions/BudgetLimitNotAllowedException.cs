namespace ModsenFinanceTracker.Domain.Exceptions;

public class BudgetLimitNotAllowedException : DomainException
{
    public string CategoryName { get; }

    public BudgetLimitNotAllowedException(string categoryName)
        : base($"Cannot set a budget limit for an income category '{categoryName}'")
    {
        CategoryName = categoryName;
    }
}
