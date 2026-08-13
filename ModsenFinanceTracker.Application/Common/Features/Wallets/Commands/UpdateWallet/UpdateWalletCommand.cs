using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.UpdateWallet;

public record UpdateWalletCommand(Guid Id, string NewName)
    : IRequest;
