using FluentValidation;

namespace ModsenFinanceTracker.Application.Common.Features.Wallets.Queries.GetWalletsPaged;

public class GetWalletsPagedQueryValidator 
    : AbstractValidator<GetWalletsPagedQuery>
{
    private const int MinPageNumber = 1;
    private const int MinPageSize = 1;
    private const int MaxPageSize = 100;

    public GetWalletsPagedQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(MinPageNumber)
            .WithMessage($"Page number must be at least {MinPageNumber}");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(MinPageSize)
            .WithMessage($"Page size must be at least {MinPageSize}")
            .LessThanOrEqualTo(MaxPageSize)
            .WithMessage($"Page size cannot exceed {MaxPageSize}");
    }
}
