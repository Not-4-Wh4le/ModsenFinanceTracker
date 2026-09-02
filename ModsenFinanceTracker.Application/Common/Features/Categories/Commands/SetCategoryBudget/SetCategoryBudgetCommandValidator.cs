using FluentValidation;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.SetCategoryBudget;

public class SetCategoryBudgetCommandValidator 
    : AbstractValidator<SetCategoryBudgetCommand>
{
    public SetCategoryBudgetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Category ID is required");

        RuleFor(x => x.NewLimit)
            .GreaterThanOrEqualTo(0)
            .When(x => x.NewLimit.HasValue)
            .WithMessage("Budget limit cannot be negative");
    }
}
