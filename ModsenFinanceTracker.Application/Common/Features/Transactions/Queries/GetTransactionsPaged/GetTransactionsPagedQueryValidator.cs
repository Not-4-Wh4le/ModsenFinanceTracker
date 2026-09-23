using FluentValidation;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.GetTransactionsPaged;

public class GetTransactionsPagedQueryValidator : AbstractValidator<GetTransactionsPagedQuery>
{
    private const int MinPageNumber = 1;
    private const int MinPageSize = 1;
    private const int MaxPageSize = 100;

    public GetTransactionsPagedQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(MinPageNumber)
            .WithMessage($"Page number must be at least {MinPageNumber}");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(MinPageSize)
            .WithMessage($"Page size must be at least {MinPageSize}")
            .LessThanOrEqualTo(MaxPageSize)
            .WithMessage($"Page size cannot exceed {MaxPageSize}");

        RuleFor(x => x.WalletId)
            .NotEmpty()
            .When(x => x.WalletId.HasValue)
            .WithMessage("Wallet ID cannot be empty if provided");

        RuleFor(x => x.TransactionType)
            .IsInEnum()
            .When(x => x.TransactionType.HasValue)
            .WithMessage("Invalid transaction type");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date");
    }
}
