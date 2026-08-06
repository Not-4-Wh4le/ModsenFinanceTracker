using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoriesPaged;

public record CategoryDto(
    Guid Id, 
    string Name, 
    string TransactionType,
    decimal? BudgetLimit = null);
