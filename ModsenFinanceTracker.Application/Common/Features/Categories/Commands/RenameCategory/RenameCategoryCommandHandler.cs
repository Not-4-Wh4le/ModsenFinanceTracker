using MediatR;
using ModsenFinanceTracker.Application.Common.Exceptions;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.UpdateCategory;

public class RenameCategoryCommandHandler
    : IRequestHandler<RenameCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public RenameCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(RenameCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), request.Id);

        category.Name = request.NewName;

        await _categoryRepository.SaveAsync(category, cancellationToken);
    }
}
