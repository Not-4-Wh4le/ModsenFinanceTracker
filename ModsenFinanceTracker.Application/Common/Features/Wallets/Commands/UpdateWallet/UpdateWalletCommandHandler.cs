using MediatR;
using ModsenFinanceTracker.Application.Common.Exceptions;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.UpdateWallet;

public class UpdateWalletCommandHandler
    : IRequestHandler<UpdateWalletCommand>
{
    private readonly IWalletRepository _walletRepository;

    public UpdateWalletCommandHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Wallet), request.Id);

        wallet.Name = request.NewName;

        await _walletRepository.SaveAsync(wallet, cancellationToken);
    }
}
