namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Queries.GetWalletDetails;

public record WalletDetailsDto(
    Guid Id,
    string Name,
    decimal Balance);
