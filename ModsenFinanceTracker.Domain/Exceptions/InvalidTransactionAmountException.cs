namespace ModsenFinanceTracker.Domain.Exceptions;

public class InvalidTransactionAmountException : DomainException
{
    public decimal AttemptedAmount { get; }

    public InvalidTransactionAmountException(decimal attemptedAmount)
        : base($"Transaction amount must be positive. Received: {attemptedAmount}")
    {
        AttemptedAmount = attemptedAmount;
    }
}
