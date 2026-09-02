using FluentValidation;
using ModsenFinanceTracker.Application.Common.Features.Categories.Commands.UpdateCategory;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.RenameCategory;

public class RenameCategoryCommandValidator 
    : AbstractValidator<RenameCategoryCommand>
{
    public RenameCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Category ID is required");

        RuleFor(x => x.NewName)
            .NotEmpty()
            .WithMessage("Name cannot be null or empty.")
            .MaximumLength(Category.MaxNameLength)
            .WithMessage($"Length cannot be longer than {Category.MaxNameLength} characters");
    }
}
