namespace KameliaRecycleSystem.Configuration;

public class ThemeConfig
{
    public bool Enabled { get; set; } = true;
    public string Description { get; set; } = nameof(ThemeConfig);
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
