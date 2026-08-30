using ModsenFinanceTracker.Application.Common.Interfaces.ReportExportStrategy;
using ModsenFinanceTracker.Application.Common.Models;
using System.Runtime.CompilerServices;
using System.Text;

namespace ModsenFinanceTracker.Application.Common.Export;

public class TxtReportExportStrategy : IReportExportStrategy
{
    private const string ContentType = "text/plain";
    private const string Etc = "...";
    private const string ColSeparator = " | ";
    private const char RowSeparator = '-';
    private const int ColWidth = 25;

    public string Format => "txt";

    public async Task<ExportFileResult> ExportAsync<T>(
        IAsyncEnumerable<T> data,
        string reportName,
        CancellationToken cancellationToken = default)
    {
        var properties = typeof(T).GetProperties();
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, leaveOpen: true);

        int totalWidth = properties.Length * ColWidth + ((properties.Length - 1) * ColSeparator.Length);
        string separator = new(RowSeparator, totalWidth);

        await writer.WriteLineAsync($"{reportName.ToUpper()}");
        await writer.WriteLineAsync($"{DateTime.Now}");
        await writer.WriteLineAsync(separator);

        var headers = string.Join(ColSeparator, properties.Select(p =>
            TruncateOrPad(p.Name, ColWidth)));
        await writer.WriteLineAsync(headers);
        await writer.WriteLineAsync(separator);

        await foreach(var item in data)
        {
            var vals = properties.Select(p => 
                TruncateOrPad(p.GetValue(item)?.ToString() ?? string.Empty, ColWidth));

            await writer.WriteLineAsync(string.Join(ColSeparator, vals));
        }

        await writer.WriteLineAsync(separator);

        await writer.FlushAsync(cancellationToken);
        stream.Position = 0;
        
        return new ExportFileResult(
            stream,
            ContentType,
            $"{reportName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{Format}");
    }

    private string TruncateOrPad(string value, int maxLength)
    {
        if (value.Length > maxLength)
        {
            return value.Substring(0, maxLength - Etc.Length) + Etc;
        }

        return value.PadRight(maxLength);
    }
}
