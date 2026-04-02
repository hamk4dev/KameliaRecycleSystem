namespace KameliaRecycleSystem.Utilities.Extensions;

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();
}
