using MediatR;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    TransactionType Type,
    decimal Amount,
    Guid CategoryId,
    string Description,
    DateTime? DateTime = null)
    : IRequest<Guid>;
