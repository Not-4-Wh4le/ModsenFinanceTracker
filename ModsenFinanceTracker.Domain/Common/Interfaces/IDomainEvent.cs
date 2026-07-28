using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Common.Interfaces
{
    public interface IDomainEvent 
    {
        DateTime Timestamp { get; }
    }
}
