namespace KameliaRecycleSystem.Reporting.Generators;

public class AuditReportGenerator
{
    public string GenerateSummary(DateTime startDate, DateTime endDate)
    {
        return $"{nameof(AuditReportGenerator)} for period {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
    }
}
