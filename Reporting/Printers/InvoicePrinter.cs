namespace KameliaRecycleSystem.Reporting.Printers;

public class InvoicePrinter
{
    public string PrepareJob(string documentName)
    {
        return $"{nameof(InvoicePrinter)}:{documentName}";
    }
}
