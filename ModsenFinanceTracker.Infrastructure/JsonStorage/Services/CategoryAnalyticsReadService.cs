using ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoryExpensesAnalytics;
using ModsenFinanceTracker.Application.Common.Interfaces.Services;
using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Services;

public class CategoryAnalyticsReadService : ICategoryAnalyticsReadService
{
    private readonly JsonDbContext _context;

    public CategoryAnalyticsReadService(JsonDbContext context)
    {
        _context = context;
    }

    public Task<CategoryExpenseAnalyticsDto> GetCategoryExpensesSummaryAsync(
        Guid? walletId, 
        DateTime? startDate, 
        DateTime? endDate, 
        CancellationToken cancellationToken = default)
    {
        var transactions = _context.Wallets
            .Where(w => !walletId.HasValue || walletId.Value == w.Id)
            .SelectMany(t => t.Transactions)
            .Where(t => t.Category.TransactionType == TransactionType.Expense)
            .Where(t => !startDate.HasValue || startDate.Value <= t.DateTime)
            .Where(t => !endDate.HasValue || endDate.Value >= t.DateTime)
            .ToList();

        var totalSum = transactions.Sum(t => t.Amount);

        if(totalSum == 0)
        {
            return Task.FromResult(new CategoryExpenseAnalyticsDto(0, Array.Empty<CategoryExpenseItemDto>()));
        }

        var items = transactions
            .GroupBy(t => new { t.Category.Id, t.Category.Name })
            .Select(g =>
            {
                var categorySum = g.Sum(t => t.Amount);
                var percentage = (double)(categorySum / totalSum * 100);
                return new CategoryExpenseItemDto(
                    g.Key.Id,
                    g.Key.Name,
                    categorySum,
                    percentage);
            })
            .OrderByDescending(i => i.TotalAmount)
            .ToList();

        return Task.FromResult(new CategoryExpenseAnalyticsDto(totalSum, items));
    }
}
