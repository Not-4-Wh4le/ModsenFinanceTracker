namespace ModsenFinanceTracker.Domain.Exceptions;

public class InsufficientFundsException : DomainException
{
    public decimal CurrentBalance { get; }
    public decimal RequestedAmount { get; }
    public InsufficientFundsException(decimal currentBalance, decimal requestedAmount)
        : base($"Insufficient funds. Current balance: {currentBalance}. Requested amount: {requestedAmount}")
    {
        CurrentBalance = currentBalance;
        RequestedAmount = requestedAmount;
    }
}