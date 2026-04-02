namespace KameliaRecycleSystem.Presentation.ViewModels;

public class DashboardViewModel
{
    public string Title { get; set; } = nameof(DashboardViewModel);
    public string Summary { get; set; } = "Scaffold view model";
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
}
