using ModsenFinanceTracker.Domain.Common;
using ModsenFinanceTracker.Domain.Exceptions;

namespace ModsenFinanceTracker.Domain.Entities;

public abstract class Transaction : Entity
{
    private string _description;

    public const int MaxDescriptionLength = 200;

    public decimal Amount { get; init; }
    public DateTime DateTime { get; init; }
    public Category Category { get; init; }
    public string Description
    {
        get
        {
            return _description;
        }
        set
        {

            if (value.Length > MaxDescriptionLength)
            {
                throw new DomainValidationException(nameof(Description) ,$"Length cannot be longer than {MaxDescriptionLength} characters");
            }
            _description = value;
        }
    }

    protected Transaction(
        Guid id, 
        decimal amount,  
        Category category, 
        string description, 
        DateTime? dateTime = null)
    {
        if (amount <= 0)
        {
            throw new InvalidTransactionAmountException(amount);
        }
        
        Id = id;
        Amount = amount;
        Category = category
            ?? throw new ArgumentNullException("Category cannot be null");
        DateTime = dateTime ?? DateTime.UtcNow;
        Description = description;
    }

    public abstract decimal Contribution();
}
