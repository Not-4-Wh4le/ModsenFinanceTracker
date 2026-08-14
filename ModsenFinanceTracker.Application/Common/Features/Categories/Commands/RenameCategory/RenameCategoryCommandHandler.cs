using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;

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
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category == null)
        {
            return;
        }

        if(category.Name == request.NewName)
        {
            return;
        }

        category.Name = request.NewName;

        await _categoryRepository.SaveAsync(category, cancellationToken);
    }
}
