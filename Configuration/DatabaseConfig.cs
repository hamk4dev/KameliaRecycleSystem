namespace KameliaRecycleSystem.Configuration;

public class DatabaseConfig
{
    public bool Enabled { get; set; } = true;
    public string Description { get; set; } = nameof(DatabaseConfig);
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
