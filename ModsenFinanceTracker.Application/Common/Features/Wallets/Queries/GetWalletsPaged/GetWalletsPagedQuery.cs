using MediatR;
using ModsenFinanceTracker.Application.Common.Models;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Queries.GetWalletsPaged;

public record GetWalletsPagedQuery(
    int PageNumber = 1,
    int PageSize = 10)
    : IRequest<PagedResult<WalletDto>>;
