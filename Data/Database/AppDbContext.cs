namespace KameliaRecycleSystem.Data.Database;

public class AppDbContext
{
    public string DatabaseName { get; set; } = "KameliaRecycleSystem";
    public string ConnectionString { get; set; } = "Data Source=DataStorage\\Database\\kamelia.db";
}
