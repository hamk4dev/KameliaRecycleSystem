namespace KameliaRecycleSystem.Utilities.Logging;

public class PerformanceMonitor
{
    public string Write(string message)
    {
        return $"{DateTime.UtcNow:O}|{nameof(PerformanceMonitor)}|{message}";
    }
}
