namespace KameliaRecycleSystem.Utilities.Extensions;

public static class StringExtensions
{
    public static string SafeTrim(this string value) => value?.Trim() ?? string.Empty;
}
