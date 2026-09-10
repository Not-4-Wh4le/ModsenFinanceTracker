using MediatR;

namespace ModsenFinanceTracker.TelegramBot.Commands;

public class ExportCsvCommandHandler : BaseExportCommandHandler
{
    public override string CommandName => "/export_csv";
    protected override string Format => "csv";

    public ExportCsvCommandHandler(
        IMediator mediator,
        ILogger<ExportCsvCommandHandler> logger)
        : base(mediator, logger)
    {
    }
}
