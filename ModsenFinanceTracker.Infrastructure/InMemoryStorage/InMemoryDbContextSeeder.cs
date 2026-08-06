using ModsenFinanceTracker.Domain.Entities;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Infrastructure.InMemoryStorage;

public static class InMemoryDbContextSeeder
{
    public static void Seed(InMemoryDbContext context)
    {
        if(context.Wallets.Any() || context.Categories.Any())
        {
            return;
        }

        var salaryCategory = new Category(Guid.NewGuid(), "Зарплата", TransactionType.Income);
        var productsCategory = new Category(Guid.NewGuid(), "Продукты", TransactionType.Expense);
        var entertainmentCategory = new Category(Guid.NewGuid(), "Развлечения", TransactionType.Expense);

        context.Categories.Add(salaryCategory);
        context.Categories.Add(productsCategory);
        context.Categories.Add(entertainmentCategory);

        var wallet = new Wallet() { Id = Guid.NewGuid()};

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

        context.Wallets.Add(wallet);
    }
}
