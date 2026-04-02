namespace KameliaRecycleSystem.Reporting.Exporters;

public class PdfExporter
{
    public string BuildFileName(string reportName)
    {
        return $"{reportName}_{DateTime.Now:yyyyMMddHHmmss}";
    }
}
