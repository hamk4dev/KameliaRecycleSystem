namespace KameliaRecycleSystem.Data.Repositories;

public class UserRepository
{
    public string RepositoryName => nameof(UserRepository);

    public IReadOnlyList<string> GetSupportedOperations()
    {
        return new[] { "Create", "Read", "Update", "Delete" };
    }
}
