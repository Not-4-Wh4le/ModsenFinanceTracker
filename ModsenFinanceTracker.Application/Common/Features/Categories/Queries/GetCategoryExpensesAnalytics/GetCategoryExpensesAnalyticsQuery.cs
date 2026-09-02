using MediatR;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoryExpensesAnalytics;

public record GetCategoryExpensesAnalyticsQuery(
    Guid? WalletId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null) 
    : IRequest<CategoryExpenseAnalyticsDto>;
