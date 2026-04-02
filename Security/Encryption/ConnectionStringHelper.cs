namespace KameliaRecycleSystem.Security.Encryption;

public class ConnectionStringHelper
{
    public string MaskPassword(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return string.Empty;
        }

        return connectionString.Replace("Password=", "Password=***", StringComparison.OrdinalIgnoreCase);
    }
}
