namespace KameliaRecycleSystem.Reporting.Generators;

public class WargaReportGenerator
{
    public string GenerateSummary(DateTime startDate, DateTime endDate)
    {
        return $"{nameof(WargaReportGenerator)} for period {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
    }
}
