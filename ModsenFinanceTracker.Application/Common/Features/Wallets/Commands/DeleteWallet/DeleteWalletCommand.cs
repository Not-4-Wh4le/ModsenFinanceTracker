using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.DeleteWallet;

public record DeleteWalletCommand(Guid Id)
    : IRequest;
