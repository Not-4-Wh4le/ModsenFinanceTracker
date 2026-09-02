using FluentValidation;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.DeleteWallet;

public class DeleteWalletCommandValidator 
    : AbstractValidator<DeleteWalletCommand>
{
    public DeleteWalletCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Wallet ID is required");
    }
}
