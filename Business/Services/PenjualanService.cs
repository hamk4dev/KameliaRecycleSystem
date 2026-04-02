namespace KameliaRecycleSystem.Business.Services;

public class PenjualanService
{
    public decimal CalculateRevenue(decimal quantity, decimal unitPrice)
    {
        return quantity * unitPrice;
    }
}
