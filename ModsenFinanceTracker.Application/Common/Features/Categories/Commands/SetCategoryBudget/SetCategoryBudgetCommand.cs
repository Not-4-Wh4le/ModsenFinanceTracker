using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.SetCategoryBudget;

public record SetCategoryBudgetCommand(Guid Id, decimal? NewLimit)
    : IRequest;
