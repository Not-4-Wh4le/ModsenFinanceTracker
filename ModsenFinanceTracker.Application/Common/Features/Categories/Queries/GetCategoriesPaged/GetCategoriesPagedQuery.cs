using MediatR;
using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.Domain.Enums;

namespace ModsenFinanceTracker.Application.Common.Features.Categories.Queries.GetCategoriesPaged;

public record GetCategoriesPagedQuery(
    TransactionType? TransactionType = null,
    int PageNumber = 1,
    int PageSize = 10) 
    : IRequest<PagedResult<CategoryDto>>;