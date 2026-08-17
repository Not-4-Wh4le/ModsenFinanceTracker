using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id)
    : IRequest;
