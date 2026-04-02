namespace KameliaRecycleSystem.Reporting.Generators;

public class SampahReportGenerator
{
    public string GenerateSummary(DateTime startDate, DateTime endDate)
    {
        return $"{nameof(SampahReportGenerator)} for period {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
    }
}
