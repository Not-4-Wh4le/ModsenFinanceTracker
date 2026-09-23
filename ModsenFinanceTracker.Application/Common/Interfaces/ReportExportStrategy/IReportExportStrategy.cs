using ModsenFinanceTracker.Application.Common.Models;

namespace ModsenFinanceTracker.Application.Common.Interfaces.ReportExportStrategy;

public interface IReportExportStrategy
{
    string Format { get; }

    Task<ExportFileResult> ExportAsync<T>(
        IAsyncEnumerable<T> data,
        string reportName,
        CancellationToken cancellationToken = default);
}
