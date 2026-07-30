using MediatR;
using ModsenFinanceTracker.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.GetTransactionsPaged;

public record GetTransactionsPagedQuery(
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int PageNumber = 1,
    int PageSize = 10)
    : IRequest<PagedResultDto<TransactionDto>>;
