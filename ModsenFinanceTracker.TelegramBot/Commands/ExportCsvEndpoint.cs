using MediatR;

namespace ModsenFinanceTracker.TelegramBot.Commands;

public class ExportCsvEndpoint : BaseExportEndpoint
{
    public override string CommandName => "/export_csv";
    protected override string Format => "csv";

    public ExportCsvEndpoint(
        IMediator mediator,
        ILogger<ExportCsvEndpoint> logger)
        : base(mediator, logger)
    {
    }
}
