namespace KameliaRecycleSystem.Business.Calculators;

public class SalaryCalculator
{
    public decimal Calculate(decimal gajiPokok, decimal bonus, decimal potongan) => gajiPokok + bonus - potongan;
}
