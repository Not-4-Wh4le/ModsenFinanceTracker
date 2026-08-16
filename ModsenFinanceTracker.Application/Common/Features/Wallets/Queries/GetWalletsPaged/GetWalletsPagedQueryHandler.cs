using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Models;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Queries.GetWalletsPaged;

public class GetWalletsPagedQueryHandler
    : IRequestHandler<GetWalletsPagedQuery, PagedResult<WalletDto>>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletsPagedQueryHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<PagedResult<WalletDto>> Handle(GetWalletsPagedQuery request, CancellationToken cancellationToken)
    {
        var (wallets, totalCount) = await _walletRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var items = wallets
            .Select(w => new WalletDto(w.Id, w.Name))
            .ToList();

        return new PagedResult<WalletDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
