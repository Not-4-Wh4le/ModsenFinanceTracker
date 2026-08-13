using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.CreateWallet;

public class CreateWalletCommandHandler
    : IRequestHandler<CreateWalletCommand, Guid>
{
    private readonly IWalletRepository _walletRepository;
    
    public CreateWalletCommandHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = new Wallet(Guid.NewGuid(), request.Name);

        await _walletRepository.SaveAsync(wallet, cancellationToken);

        return wallet.Id;
    }
}
