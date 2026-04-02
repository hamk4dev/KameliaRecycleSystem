namespace KameliaRecycleSystem.Utilities.Logging;

public class Logger
{
    public string Write(string message)
    {
        return $"{DateTime.UtcNow:O}|{nameof(Logger)}|{message}";
    }
}
