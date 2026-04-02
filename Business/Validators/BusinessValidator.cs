namespace KameliaRecycleSystem.Business.Validators;

public class BusinessValidator
{
    public bool ValidatePositive(decimal amount) => amount >= 0;
}
