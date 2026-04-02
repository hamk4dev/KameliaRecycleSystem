namespace KameliaRecycleSystem.Reporting.Printers;

public class ReportPrinter
{
    public string PrepareJob(string documentName)
    {
        return $"{nameof(ReportPrinter)}:{documentName}";
    }
}
