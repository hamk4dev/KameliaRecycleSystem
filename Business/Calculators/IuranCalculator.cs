namespace KameliaRecycleSystem.Business.Calculators;

public class IuranCalculator
{
    public decimal Calculate(decimal iuranBulanan, int jumlahBulan) => iuranBulanan * jumlahBulan;
}
