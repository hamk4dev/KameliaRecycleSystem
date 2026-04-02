namespace KameliaRecycleSystem.Configuration;

public class ReportConfig
{
    public bool Enabled { get; set; } = true;
    public string Description { get; set; } = nameof(ReportConfig);
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
