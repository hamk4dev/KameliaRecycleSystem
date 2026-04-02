namespace KameliaRecycleSystem.Security.Authorization;

public class AuditLogger
{
    public string CreateLogEntry(string actor, string action)
    {
        return $"{DateTime.UtcNow:O}|{actor}|{action}";
    }
}
