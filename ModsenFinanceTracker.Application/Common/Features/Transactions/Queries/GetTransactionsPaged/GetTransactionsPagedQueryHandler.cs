using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Models;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.GetTransactionsPaged;

public class GetTransactionsPagedQueryHandler : IRequestHandler<GetTransactionsPagedQuery, PagedResult<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    
    public GetTransactionsPagedQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<PagedResult<TransactionDto>> Handle(GetTransactionsPagedQuery request, CancellationToken cancellationToken)
    {
        var pagedTransactions = await _transactionRepository.GetPagedAsync(
            request.WalletId,
            request.TransactionType,
            request.StartDate,
            request.EndDate,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var dtos = pagedTransactions.Items
            .Select(t => new TransactionDto(
                t.Id,
                t.Amount,
                t.Category.TransactionType.ToString(),
                t.Category.Name,
                t.Description,
                t.DateTime))
            .ToList();

        var result = new PagedResult<TransactionDto>(
            dtos,
            pagedTransactions.TotalCount,
            pagedTransactions.PageNumber,
            pagedTransactions.PageSize);

        return result;
    }
}
