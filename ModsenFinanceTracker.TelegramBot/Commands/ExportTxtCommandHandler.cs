using MediatR;

namespace ModsenFinanceTracker.TelegramBot.Commands;

public class ExportTxtCommandHandler : BaseExportCommandHandler
{
    public override string CommandName => "/export_txt";
    protected override string Format => "txt";

    public ExportTxtCommandHandler(
        IMediator mediator,
        ILogger<ExportTxtCommandHandler> logger)
        : base(mediator, logger)
    {
    }
}