using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Common.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; init; }
    }
}
