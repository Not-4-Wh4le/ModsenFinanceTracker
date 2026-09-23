using MediatR;
using ModsenFinanceTracker.Application.Common.Interfaces.ReportExportStrategy;
using ModsenFinanceTracker.Application.Common.Interfaces.Repositories;
using ModsenFinanceTracker.Application.Common.Models;
using ModsenFinanceTracker.Domain.Entities;
using System.Runtime.CompilerServices;

namespace ModsenFinanceTracker.Application.Common.Features.Transactions.Queries.ExportTransactions;

public class ExportTransactionsQueryHandler : IRequestHandler<ExportTransactionsQuery, ExportFileResult>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IReportExportFactory _exporterFactory;

    public const string BaseFileName = "TransactionsReport";
    public const string ExportDataFormat = "dd.MM.yyyy HH:mm:ss";

    public ExportTransactionsQueryHandler(
        ITransactionRepository transactionRepository,
        IReportExportFactory exporterFactory)
    {
        _transactionRepository = transactionRepository;
        _exporterFactory = exporterFactory;
    }

    public async Task<ExportFileResult> Handle(
        ExportTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactionsStream = _transactionRepository.GetTransactionsAsync(
            request.WalletId,
            request.TransactionType,
            request.StartDate,
            request.EndDate,
            cancellationToken);


        var strategy = _exporterFactory.GetStrategy(request.Format);

        var reportData = MapAsync(transactionsStream, cancellationToken);

        return await strategy.ExportAsync(reportData, BaseFileName, cancellationToken);
    }

    private async IAsyncEnumerable<ExportTransactionDto> MapAsync(
        IAsyncEnumerable<Transaction> transactions,
        [EnumeratorCancellation ]CancellationToken cancellationToken)
    {
        await foreach(var transaction in transactions.WithCancellation(cancellationToken))
        {
            yield return new ExportTransactionDto(
                transaction.Id,
                transaction.DateTime.ToString(ExportDataFormat),
                transaction.Amount,
                transaction.Category.Name,
                transaction.Description);
        }
    }
}
