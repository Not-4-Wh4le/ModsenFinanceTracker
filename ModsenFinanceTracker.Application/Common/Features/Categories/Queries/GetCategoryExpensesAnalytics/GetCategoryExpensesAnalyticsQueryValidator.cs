using FluentValidation;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoryExpensesAnalytics;

public class GetCategoryExpensesAnalyticsQueryValidator 
    : AbstractValidator<GetCategoryExpensesAnalyticsQuery>
{
    public GetCategoryExpensesAnalyticsQueryValidator()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty()
            .When(x => x.WalletId.HasValue)
            .WithMessage("Wallet ID cannot be empty if provided");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date");
    }
}
