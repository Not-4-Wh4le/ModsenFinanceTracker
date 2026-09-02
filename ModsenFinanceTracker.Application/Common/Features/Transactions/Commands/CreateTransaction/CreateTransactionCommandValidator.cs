using FluentValidation;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator 
    : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid transaction type");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.WalletId)
            .NotEmpty()
            .WithMessage("Wallet ID is required");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID is required");

        RuleFor(x => x.Description)
            .MaximumLength(Transaction.MaxDescriptionLength)
            .WithMessage($"Description length cannot exceed {Transaction.MaxDescriptionLength} characters");
    }
}
