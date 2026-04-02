namespace KameliaRecycleSystem.Reporting.Generators;

public class OperationalReportGenerator
{
    public string GenerateSummary(DateTime startDate, DateTime endDate)
    {
        return $"{nameof(OperationalReportGenerator)} for period {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
    }
}
