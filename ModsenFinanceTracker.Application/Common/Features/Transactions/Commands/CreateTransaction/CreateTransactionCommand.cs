using MediatR;
using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    TransactionType Type,
    decimal Amount,
    Guid CategoryId,
    string Description,
    DateTime? DateTime = null)
    : IRequest<Guid>;
