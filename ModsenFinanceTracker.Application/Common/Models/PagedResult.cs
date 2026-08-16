using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Models;

public record PagedResult<T>(
        IReadOnlyList<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)(TotalCount) / PageSize);
    public bool HasNextPage => TotalPages > PageNumber;
    public bool HasPreviousPage => PageNumber > 1;
}