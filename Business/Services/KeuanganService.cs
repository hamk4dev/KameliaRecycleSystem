namespace KameliaRecycleSystem.Business.Services;

public class KeuanganService
{
    public decimal CalculateSaldo(decimal pemasukan, decimal pengeluaran)
    {
        return pemasukan - pengeluaran;
    }
}
