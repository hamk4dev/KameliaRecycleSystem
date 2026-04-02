namespace KameliaRecycleSystem.Utilities.Helpers;

public class FileHelper
{
    public string Combine(params string[] parts) => Path.Combine(parts);
}
