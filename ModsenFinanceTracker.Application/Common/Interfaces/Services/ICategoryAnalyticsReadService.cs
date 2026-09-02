using ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoryExpensesAnalytics;

namespace ModsenFinanceTracker.Application.Common.Interfaces.Services;

public interface ICategoryAnalyticsReadService
{
    Task<CategoryExpenseAnalyticsDto> GetCategoryExpensesSummaryAsync(
           Guid? walletId,
           DateTime? startDate,
           DateTime? endDate,
           CancellationToken cancellationToken = default);
}
