using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.UpdateTrancsaction;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
{
    private readonly IWalletRepository _walletRepository;

    public UpdateTransactionCommandHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAsync(request.WalletId, cancellationToken);

        if (wallet == null)
        {
            return;
        }

        wallet.UpdateTransactionDescription(request.TransactionId, request.NewDescription);

        await _walletRepository.SaveAsync(wallet, cancellationToken);
    }
}
