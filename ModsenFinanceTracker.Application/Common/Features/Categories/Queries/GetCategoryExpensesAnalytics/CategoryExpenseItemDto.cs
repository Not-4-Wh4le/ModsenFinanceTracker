namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoryExpensesAnalytics;

public record CategoryExpenseItemDto(
    Guid CategoryId,
    string CategoryName,
    decimal TotalAmount,
    double Percentage);

