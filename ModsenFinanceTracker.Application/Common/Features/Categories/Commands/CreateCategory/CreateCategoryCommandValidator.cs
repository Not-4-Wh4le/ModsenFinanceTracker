using FluentValidation;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator 
    : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be null or empty")
            .MaximumLength(Category.MaxNameLength)
            .WithMessage($"Length cannot be longer than {Category.MaxNameLength} characters");

        RuleFor(x => x.TransactionType)
            .IsInEnum()
            .WithMessage("Invalid transaction type");

        RuleFor(x => x.BudgetLimit)
            .GreaterThanOrEqualTo(0)
            .When(x => x.BudgetLimit.HasValue)
            .WithMessage("Budget limit cannot be negative");

        RuleFor(x => x.BudgetLimit)
            .Null()
            .When(x => x.TransactionType == TransactionType.Income)
            .WithMessage("Budget limit is not allowed for income categories");
    }
}
