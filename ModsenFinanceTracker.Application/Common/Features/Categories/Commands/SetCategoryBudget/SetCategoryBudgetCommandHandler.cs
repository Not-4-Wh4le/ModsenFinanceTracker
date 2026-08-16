using MediatR;
using ModsenFinanceTracker.Application.Common.Exceptions;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Entities;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Commands.SetCategoryBudget;

public class SetCategoryBudgetCommandHandler
    : IRequestHandler<SetCategoryBudgetCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public SetCategoryBudgetCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(SetCategoryBudgetCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), request.Id);

        category.BudgetLimit = request.NewLimit;

        await _categoryRepository.SaveAsync(category, cancellationToken);
    }
}
