namespace KameliaRecycleSystem.Configuration;

public class BackupConfig
{
    public bool Enabled { get; set; } = true;
    public string Description { get; set; } = nameof(BackupConfig);
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
