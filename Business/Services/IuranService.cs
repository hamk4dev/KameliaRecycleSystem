namespace KameliaRecycleSystem.Business.Services;

public class IuranService
{
    public decimal CalculateOutstanding(decimal monthlyFee, int unpaidMonths)
    {
        return monthlyFee * unpaidMonths;
    }
}
