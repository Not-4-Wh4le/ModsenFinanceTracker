using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.DeleteWallet;

public class DeleteWalletCommandHandler
    : IRequestHandler<DeleteWalletCommand>
{
    private readonly IWalletRepository _walletRepository;
    public DeleteWalletCommandHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        await _walletRepository.DeleteAsync(request.Id, cancellationToken);
    }
}