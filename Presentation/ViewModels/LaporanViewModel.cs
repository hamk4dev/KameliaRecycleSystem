namespace KameliaRecycleSystem.Presentation.ViewModels;

public class LaporanViewModel
{
    public string Title { get; set; } = nameof(LaporanViewModel);
    public string Summary { get; set; } = "Scaffold view model";
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
}
