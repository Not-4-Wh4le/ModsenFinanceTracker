using FluentValidation;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Commands.CreateWallet;

public class CreateWalletCommandValidator 
    : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be null or empty")
            .MaximumLength(Wallet.MaxNameLength)
            .WithMessage($"Length cannot be longer than {Wallet.MaxNameLength} characters");
    }
}