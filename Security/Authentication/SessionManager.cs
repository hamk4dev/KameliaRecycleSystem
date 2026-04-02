namespace KameliaRecycleSystem.Security.Authentication;

public class SessionManager
{
    private readonly Dictionary<string, DateTime> _sessions = new();

    public string CreateSession(string username)
    {
        var token = Guid.NewGuid().ToString("N");
        _sessions[token] = DateTime.UtcNow;
        return token;
    }

    public bool IsSessionActive(string token)
    {
        return _sessions.ContainsKey(token);
    }
}
