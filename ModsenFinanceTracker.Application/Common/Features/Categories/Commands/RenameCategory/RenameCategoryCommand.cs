using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.UpdateCategory;

public record RenameCategoryCommand(
    Guid Id, 
    string NewName)
    : IRequest;
