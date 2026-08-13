namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoryExpensesAnalytics;

public record CategoryExpenseAnalyticsDto(
    decimal TotalExpenseSum,
    IReadOnlyCollection<CategoryExpenseItemDto> Items
);