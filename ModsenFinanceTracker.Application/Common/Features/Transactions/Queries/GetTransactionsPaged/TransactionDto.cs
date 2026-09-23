using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.GetTransactionsPaged;

public record TransactionDto(
    Guid Id,
    decimal Amount,
    string Type,
    string CategoryName,
    string Description,
    DateTime DateTime);
