namespace KameliaRecycleSystem.Utilities.Logging;

public class LogManager
{
    public string Write(string message)
    {
        return $"{DateTime.UtcNow:O}|{nameof(LogManager)}|{message}";
    }
}
