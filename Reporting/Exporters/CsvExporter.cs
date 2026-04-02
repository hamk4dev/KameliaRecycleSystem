namespace KameliaRecycleSystem.Reporting.Exporters;

public class CsvExporter
{
    public string BuildFileName(string reportName)
    {
        return $"{reportName}_{DateTime.Now:yyyyMMddHHmmss}";
    }
}
