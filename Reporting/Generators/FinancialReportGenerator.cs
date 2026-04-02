namespace KameliaRecycleSystem.Reporting.Generators;

public class FinancialReportGenerator
{
    public string GenerateSummary(DateTime startDate, DateTime endDate)
    {
        return $"{nameof(FinancialReportGenerator)} for period {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
    }
}
