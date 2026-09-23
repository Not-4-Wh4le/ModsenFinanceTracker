using FluentValidation;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoriesPaged;

public class GetCategoriesPagedQueryValidator 
    : AbstractValidator<GetCategoriesPagedQuery>
{
    private const int MaxPageSize = 100;
    private const int MinPageSize = 1;
    private const int MinPageNumber = 1;

    public GetCategoriesPagedQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(MinPageNumber)
            .WithMessage($"Page number must be at least {MinPageNumber}");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(MinPageSize)
            .WithMessage($"Page size must be at least {MinPageSize}")
            .LessThanOrEqualTo(MaxPageSize)
            .WithMessage($"Page size cannot exceed {MaxPageSize}");

        RuleFor(x => x.TransactionType)
            .IsInEnum()
            .When(x => x.TransactionType.HasValue)
            .WithMessage("Invalid transaction type");
    }
}