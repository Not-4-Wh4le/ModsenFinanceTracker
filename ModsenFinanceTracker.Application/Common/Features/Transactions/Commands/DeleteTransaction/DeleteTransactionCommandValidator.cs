using FluentValidation;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandValidator 
    : AbstractValidator<DeleteTransactionCommand>
{
    public DeleteTransactionCommandValidator()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty()
            .WithMessage("Wallet ID is required");

        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage("Transaction ID is required");
    }
}
