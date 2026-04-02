namespace KameliaRecycleSystem.Business.Services;

public class DashboardService
{
    public IReadOnlyDictionary<string, decimal> BuildSummary(decimal saldo, decimal iuran, decimal penjualan)
    {
        return new Dictionary<string, decimal>
        {
            ["Saldo"] = saldo,
            ["Iuran"] = iuran,
            ["Penjualan"] = penjualan
        };
    }
}
