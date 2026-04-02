namespace KameliaRecycleSystem.Presentation.ViewModels;

public class PrintViewModel
{
    public string Title { get; set; } = nameof(PrintViewModel);
    public string Summary { get; set; } = "Scaffold view model";
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
}
