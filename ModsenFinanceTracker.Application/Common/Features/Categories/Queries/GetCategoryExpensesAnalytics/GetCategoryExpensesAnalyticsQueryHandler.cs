using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoryExpensesAnalytics;

public class GetCategoryExpensesAnalyticsQueryHandler
    : IRequestHandler<GetCategoryExpensesAnalyticsQuery, CategoryExpenseAnalyticsDto>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetCategoryExpensesAnalyticsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<CategoryExpenseAnalyticsDto> Handle(
        GetCategoryExpensesAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        var (transactions, _) = await _transactionRepository.GetPagedAsync(
            request.WalletId,
            request.StartDate,
            request.EndDate,
            pageNumber: 1,
            pageSize: int.MaxValue,
            cancellationToken: cancellationToken);

        var expenses = transactions
            .Where(t => t.Category.TransactionType == TransactionType.Expense)
            .ToList();

        var totalExpenseSum = expenses.Sum(t => t.Amount);

        if (totalExpenseSum == 0)
        {
            return new CategoryExpenseAnalyticsDto(0, Array.Empty<CategoryExpenseItemDto>());
        }

        var items = expenses
            .GroupBy(t => new { t.Category.Id, t.Category.Name })
            .Select(g =>
            {
                var categorySum = g.Sum(t => t.Amount);
                var percentage = (double)(categorySum / totalExpenseSum * 100);

                return new CategoryExpenseItemDto(
                    g.Key.Id,
                    g.Key.Name,
                    categorySum,
                    Math.Round(percentage, 2)
                );
            })
            .OrderByDescending(i => i.TotalAmount)
            .ToList();

        return new CategoryExpenseAnalyticsDto(totalExpenseSum, items);
    }
}
