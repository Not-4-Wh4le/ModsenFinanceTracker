using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Queries.GetWalletDetails;

public record GetWalletDetailsQuery(Guid Id)
    : IRequest<WalletDetailsDto?>;
