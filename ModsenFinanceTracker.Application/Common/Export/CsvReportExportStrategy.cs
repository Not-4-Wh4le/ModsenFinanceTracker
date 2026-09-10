using ModsenFinanceTracker.Application.Common.Interfaces.ReportExportStrategy;
using ModsenFinanceTracker.Application.Common.Models;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Export;

public class CsvReportExportStrategy : IReportExportStrategy
{
    private const string Separator = ";";
    private const string ContentType = "text/csv";
    private const string FileNameDateTimeFormat = "yyyyMMdd_HHmmss";

    public string Format => "csv";

    public async Task<ExportFileResult> ExportAsync<T>(
        IAsyncEnumerable<T> data, 
        string reportName, 
        CancellationToken cancellationToken = default)
    {
        var properties = typeof(T).GetProperties();
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);
        
        var header = string.Join(Separator, properties.Select(p => p.Name));
        
        await writer.WriteLineAsync(header);

        await foreach(var item in data.WithCancellation(cancellationToken))
        {
            var vals = properties.Select(p => p.GetValue(item)?.ToString() ?? string.Empty);
            await writer.WriteLineAsync(string.Join(Separator, vals));
        }

        await writer.FlushAsync(cancellationToken);
        stream.Position = 0;

        return new ExportFileResult(
            stream,
            ContentType,
            $"{reportName}_{DateTime.UtcNow.ToString(FileNameDateTimeFormat)}.{Format}");
    }

    
}
