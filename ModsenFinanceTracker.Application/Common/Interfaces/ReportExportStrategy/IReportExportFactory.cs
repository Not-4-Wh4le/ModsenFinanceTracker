namespace ModsenFinanceTracker.Application.Common.Interfaces.ReportExportStrategy;

public interface IReportExportFactory
{
    IReportExportStrategy GetStrategy(string format);
}
