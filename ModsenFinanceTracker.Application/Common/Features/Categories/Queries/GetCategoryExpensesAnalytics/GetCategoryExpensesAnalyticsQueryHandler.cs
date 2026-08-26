using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Interfaces.Services;
using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoryExpensesAnalytics;

public class GetCategoryExpensesAnalyticsQueryHandler
    : IRequestHandler<GetCategoryExpensesAnalyticsQuery, CategoryExpenseAnalyticsDto>
{
    private readonly ICategoryAnalyticsReadService _categoryAnalyticsReadService;

    public GetCategoryExpensesAnalyticsQueryHandler(ICategoryAnalyticsReadService categoryAnalyticsReadService)
    {
        _categoryAnalyticsReadService = categoryAnalyticsReadService;
    }

    public Task<CategoryExpenseAnalyticsDto> Handle(
        GetCategoryExpensesAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        return _categoryAnalyticsReadService.GetCategoryExpensesSummaryAsync(
            request.WalletId,
            request.StartDate,
            request.EndDate,
            cancellationToken);
        ;
    }
}
