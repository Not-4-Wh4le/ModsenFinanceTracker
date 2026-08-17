using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Models;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoriesPaged;

public class GetCategoriesPagedQueryHandler
    : IRequestHandler<GetCategoriesPagedQuery, PagedResult<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesPagedQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResult<CategoryDto>> Handle(GetCategoriesPagedQuery request, CancellationToken cancellationToken)
    {
        var pagedCategories = await _categoryRepository.GetPagedAsync(
            request.TransactionType,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var dtos = pagedCategories.Items
            .Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.TransactionType.ToString(),
                c.BudgetLimit))
            .ToList();

        var result = new PagedResult<CategoryDto>(
            dtos,
            pagedCategories.TotalCount,
            pagedCategories.PageNumber,
            pagedCategories.PageSize);

        return result;
    }
}
