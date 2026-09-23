using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.UpdateTrancsaction;

public record UpdateTransactionCommand(Guid WalletId, Guid TransactionId, string NewDescription)
    : IRequest;
