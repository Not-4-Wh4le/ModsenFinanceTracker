using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoriesPaged;

public class GetCategoriesPagedQueryHandler
    : IRequestHandler<GetCategoriesPagedQuery, PagedResultDto<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesPagedQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResultDto<CategoryDto>> Handle(GetCategoriesPagedQuery request, CancellationToken cancellationToken)
    {
        var (categories, totalCount) = await _categoryRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var items = categories
            .Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.TransactionType.ToString(),
                c.BudgetLimit))
            .ToList();

        var result = new PagedResultDto<CategoryDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return result;
    }
}
