namespace KameliaRecycleSystem.Business.Calculators;

public class SampahValueCalculator
{
    public decimal Calculate(decimal beratKg, decimal hargaPerKg) => beratKg * hargaPerKg;
}
