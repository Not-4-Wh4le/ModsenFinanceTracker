using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;

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
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if(category == null)
        {
            return;
        }

        if(category.BudgetLimit == request.NewLimit)
        {
            return;
        }

        category.BudgetLimit = request.NewLimit;

        await _categoryRepository.SaveAsync(category, cancellationToken);
    }
}
