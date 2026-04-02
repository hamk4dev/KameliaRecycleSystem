namespace KameliaRecycleSystem.Business.Services;

public class ReportService
{
    public string BuildTitle(string reportName)
    {
        return $"Laporan {reportName}";
    }
}
