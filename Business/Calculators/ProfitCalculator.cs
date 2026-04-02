namespace KameliaRecycleSystem.Business.Calculators;

public class ProfitCalculator
{
    public decimal Calculate(decimal revenue, decimal expense) => revenue - expense;
}
