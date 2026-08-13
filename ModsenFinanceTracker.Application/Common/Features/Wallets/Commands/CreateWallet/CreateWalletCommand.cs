using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.CreateWallet;

public record CreateWalletCommand(string Name)
    : IRequest<Guid>;
