using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.DeleteTransaction;

public record DeleteTransactionCommand(Guid WalletId, Guid TransactionId)
    : IRequest;
