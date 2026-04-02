namespace KameliaRecycleSystem.Configuration;

public class AppSettings
{
    public bool Enabled { get; set; } = true;
    public string Description { get; set; } = nameof(AppSettings);
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
