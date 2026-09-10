using MediatR;

namespace ModsenFinanceTracker.TelegramBot.Commands;

public class ExportTxtEndpoint : BaseExportEndpoint
{
    public override string CommandName => "/export_txt";
    protected override string Format => "txt";

    public ExportTxtEndpoint(
        IMediator mediator,
        ILogger<ExportTxtEndpoint> logger)
        : base(mediator, logger)
    {
    }
}