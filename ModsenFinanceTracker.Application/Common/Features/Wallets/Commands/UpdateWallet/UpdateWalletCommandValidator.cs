using FluentValidation;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.UpdateWallet;

public class UpdateWalletCommandValidator 
    : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Wallet ID is required");

        RuleFor(x => x.NewName)
            .NotEmpty()
            .WithMessage("Name cannot be null or empty")
            .MaximumLength(Wallet.MaxNameLength)
            .WithMessage($"Length cannot be longer than {Wallet.MaxNameLength} characters");
    }
}