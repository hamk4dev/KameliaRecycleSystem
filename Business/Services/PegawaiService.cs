namespace KameliaRecycleSystem.Business.Services;

public class PegawaiService
{
    public decimal CalculateTakeHomePay(decimal gajiPokok, decimal bonus, decimal potongan)
    {
        return gajiPokok + bonus - potongan;
    }
}
