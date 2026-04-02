namespace KameliaRecycleSystem.Utilities.Logging;

public class ErrorHandler
{
    public string Write(string message)
    {
        return $"{DateTime.UtcNow:O}|{nameof(ErrorHandler)}|{message}";
    }
}
