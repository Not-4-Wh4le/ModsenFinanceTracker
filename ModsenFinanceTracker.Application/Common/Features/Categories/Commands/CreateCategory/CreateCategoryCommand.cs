using MediatR;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name, 
    TransactionType TransactionType, 
    decimal? BudgetLimit = null)
    : IRequest<Guid>;
