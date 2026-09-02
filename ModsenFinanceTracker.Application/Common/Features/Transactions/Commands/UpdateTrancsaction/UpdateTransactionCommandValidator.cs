using FluentValidation;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.UpdateTrancsaction;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty()
            .WithMessage("Wallet ID is required");

        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage("Transaction ID is required");

        RuleFor(x => x.NewDescription)
            .MaximumLength(Transaction.MaxDescriptionLength)
            .WithMessage($"Description length cannot exceed {Transaction.MaxDescriptionLength} characters");
    }
}
