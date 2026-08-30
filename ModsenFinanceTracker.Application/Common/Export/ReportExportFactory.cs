using ModsenFinanceTracker.Application.Common.Interfaces.ReportExportStrategy;

namespace ModsenFinanceTracker.Application.Common.Export;

public class ReportExportFactory : IReportExportFactory
{
    private readonly IReadOnlyDictionary<string, IReportExportStrategy> _strategies;

    public ReportExportFactory(IEnumerable<IReportExportStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(s => s.Format);
    }

    public IReportExportStrategy GetStrategy(string format)
    {
        return _strategies[format];
    }
}