namespace KameliaRecycleSystem.Data.Repositories;

public class KeuanganRepository
{
    public string RepositoryName => nameof(KeuanganRepository);

    public IReadOnlyList<string> GetSupportedOperations()
    {
        return new[] { "Create", "Read", "Update", "Delete" };
    }
}
