using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Infrastructure.JsonStorage;

public static class JsonDbContextSeeder
{
    public static async Task SeedAsync(JsonDbContext context, CancellationToken cancellationToken = default)
    {
        if (context.Wallets.Any() || context.Categories.Any())
        {
            return;
        }

        var salaryCategory = new Category(Guid.NewGuid(), "Зарплата", TransactionType.Income);
        var productsCategory = new Category(Guid.NewGuid(), "Продукты", TransactionType.Expense);
        var entertainmentCategory = new Category(Guid.NewGuid(), "Развлечения", TransactionType.Expense);

        context.Categories.Add(salaryCategory);
        context.Categories.Add(productsCategory);
        context.Categories.Add(entertainmentCategory);

        var wallet = new Wallet(Guid.NewGuid(), "Мой кошелек");

        var incomeTransaction = new IncomeTransaction(
            Guid.NewGuid(),
            3000.00m,
            salaryCategory,
            "Зарплата",
            DateTime.UtcNow.AddDays(-10)
        );

        var expenseTransaction1 = new ExpenseTransaction(
            Guid.NewGuid(),
            100.50m,
            productsCategory,
            "Покупка продуктов",
            DateTime.UtcNow.AddDays(-3)
        );

        var expenseTransaction2 = new ExpenseTransaction(
            Guid.NewGuid(),
            16.00m,
            entertainmentCategory,
            "Билеты на Одиссею",
            DateTime.UtcNow.AddDays(-1)
        );

        wallet.AddTransaction(incomeTransaction);
        wallet.AddTransaction(expenseTransaction1);
        wallet.AddTransaction(expenseTransaction2);

        context.Transactions.Add(incomeTransaction);
        context.Transactions.Add(expenseTransaction1);
        context.Transactions.Add(expenseTransaction2);

        context.Wallets.Add(wallet);

        await context.SaveChangesAsync(cancellationToken);
    }
}
