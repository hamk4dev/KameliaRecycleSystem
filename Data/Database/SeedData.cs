namespace KameliaRecycleSystem.Data.Database;

public class SeedData
{
    public IReadOnlyList<string> GetDefaultUsers()
    {
        return new[] { "admin", "operator", "warga" };
    }
}
