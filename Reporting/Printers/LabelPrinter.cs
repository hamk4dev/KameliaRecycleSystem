namespace KameliaRecycleSystem.Reporting.Printers;

public class LabelPrinter
{
    public string PrepareJob(string documentName)
    {
        return $"{nameof(LabelPrinter)}:{documentName}";
    }
}
