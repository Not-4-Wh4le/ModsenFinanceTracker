namespace ModsenFinanceTracker.Infrastructure.JsonStorage.Models;

public class CategoryDataModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty; 
    public decimal? BudgetLimit { get; set; }
}
