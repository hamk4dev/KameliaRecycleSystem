namespace KameliaRecycleSystem.Data.Repositories;

public class PegawaiRepository
{
    public string RepositoryName => nameof(PegawaiRepository);

    public IReadOnlyList<string> GetSupportedOperations()
    {
        return new[] { "Create", "Read", "Update", "Delete" };
    }
}
