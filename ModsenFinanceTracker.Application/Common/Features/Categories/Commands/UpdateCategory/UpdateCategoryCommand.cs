using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name, decimal BudgetLimit)
    : IRequest;
