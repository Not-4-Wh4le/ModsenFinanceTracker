using FluentValidation;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Queries.GetWalletDetails;

public class GetWalletDetailsQueryValidator 
    : AbstractValidator<GetWalletDetailsQuery>
{
    public GetWalletDetailsQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Wallet ID is required");
    }
}
