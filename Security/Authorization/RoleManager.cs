namespace KameliaRecycleSystem.Security.Authorization;

public class RoleManager
{
    public string NormalizeRole(string role)
    {
        return string.IsNullOrWhiteSpace(role) ? "Guest" : role.Trim();
    }
}
