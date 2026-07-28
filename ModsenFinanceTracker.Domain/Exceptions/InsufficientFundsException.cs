using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public decimal CurrentBalance { get; }
        public decimal RequestedAmount { get; }
        public InsufficientFundsException(decimal currentBalance, decimal requestedAmount)
            : base($"Insufficient funds. Current balance: {currentBalance}, requested amount: {requestedAmount}")
        {
            CurrentBalance = currentBalance;
            RequestedAmount = requestedAmount;
        }
    }
}
