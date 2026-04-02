namespace KameliaRecycleSystem.Security.Authorization;

public class PermissionService
{
    public bool CanAccess(string role, string moduleName)
    {
        if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(moduleName);
    }
}
