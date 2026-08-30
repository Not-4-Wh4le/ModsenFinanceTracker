namespace ModsenFinanceTracker.Application.Common.Models;

public record ExportFileResult(
    Stream FileContents,
    string ContentType,
    string FileName);
