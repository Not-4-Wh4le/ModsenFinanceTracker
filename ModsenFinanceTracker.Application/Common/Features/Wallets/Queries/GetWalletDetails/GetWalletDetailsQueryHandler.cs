using MediatR;
using ModsenFinanceTracker.Application.Common.Exceptions;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Queries.GetWalletDetails;

public class GetWalletDetailsQueryHandler
    : IRequestHandler<GetWalletDetailsQuery, WalletDetailsDto?>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletDetailsQueryHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<WalletDetailsDto?> Handle(GetWalletDetailsQuery request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Wallet), request.Id); 

        return new WalletDetailsDto(
            wallet.Id,
            wallet.Name,
            wallet.Balance
        );
    }
}
