namespace KameliaRecycleSystem.Business.Services;

public class SampahService
{
    public decimal CalculateProcessedWeight(decimal organikKg, decimal anorganikKg)
    {
        return organikKg + anorganikKg;
    }
}
