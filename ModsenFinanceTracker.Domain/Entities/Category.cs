using ModsenFinanceTracker.Domain.Common;
using ModsenFinanceTracker.Domain.Enums;
using ModsenFinanceTracker.Domain.Exceptions;

namespace ModsenFinanceTracker.Domain.Entities;

public class Category : Entity
{
    private decimal? _budgetLimit;
    private string _name;

    public const int MaxNameLength = 50;

    public TransactionType TransactionType { get; init; }
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException(nameof(Name), "Name cannot be null or empty");
            }

            if (value.Length > MaxNameLength)
            {
                throw new DomainValidationException(nameof(Name) ,$"Length cannot be longer than {MaxNameLength} characters");
            }
            _name = value;
        }

    }
    public decimal? BudgetLimit
    {
        get
        {
            return _budgetLimit;
        }
        set
        {
            if(value.HasValue && value < 0)
            {
                throw new InvalidCategoryBudgetLimitException(value.Value);
            }

            if(value.HasValue && TransactionType == TransactionType.Income)
            {
                throw new BudgetLimitNotAllowedException(Name);
            }

            _budgetLimit = value;
        }
    }

    public Category(Guid id, string name, TransactionType transactionType, decimal? budgetLimit = null)
    {
        Id = id;
        TransactionType = transactionType;
        Name = name;
        BudgetLimit = budgetLimit;
    }
}