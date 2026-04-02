namespace KameliaRecycleSystem.Configuration;

public class SecurityConfig
{
    public bool Enabled { get; set; } = true;
    public string Description { get; set; } = nameof(SecurityConfig);
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
