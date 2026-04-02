namespace KameliaRecycleSystem.Core.Exceptions;

public class DatabaseException : DomainException
{
    public DatabaseException()
        : base("DATABASE_ERROR", "Terjadi kesalahan pada database", "Terjadi kesalahan saat mengakses database")
    {
    }

    public DatabaseException(string message)
        : base("DATABASE_ERROR", message, "Terjadi kesalahan saat mengakses database")
    {
    }

    public DatabaseException(string message, Exception innerException)
        : base("DATABASE_ERROR", message, innerException, "Terjadi kesalahan saat mengakses database")
    {
    }
}
