namespace KameliaRecycleSystem.Data.Repositories;

public class SampahRepository
{
    public string RepositoryName => nameof(SampahRepository);

    public IReadOnlyList<string> GetSupportedOperations()
    {
        return new[] { "Create", "Read", "Update", "Delete" };
    }
}
