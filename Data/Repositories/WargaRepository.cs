namespace KameliaRecycleSystem.Data.Repositories;

public class WargaRepository
{
    public string RepositoryName => nameof(WargaRepository);

    public IReadOnlyList<string> GetSupportedOperations()
    {
        return new[] { "Create", "Read", "Update", "Delete" };
    }
}
