namespace KameliaRecycleSystem.Reporting.Exporters;

public class ExcelExporter
{
    public string BuildFileName(string reportName)
    {
        return $"{reportName}_{DateTime.Now:yyyyMMddHHmmss}";
    }
}
