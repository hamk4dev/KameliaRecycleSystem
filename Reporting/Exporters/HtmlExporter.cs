namespace KameliaRecycleSystem.Reporting.Exporters;

public class HtmlExporter
{
    public string BuildFileName(string reportName)
    {
        return $"{reportName}_{DateTime.Now:yyyyMMddHHmmss}";
    }
}
