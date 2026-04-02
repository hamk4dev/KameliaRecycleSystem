namespace KameliaRecycleSystem.Business.Validators;

public class InputValidator
{
    public bool ValidateRequired(string value) => !string.IsNullOrWhiteSpace(value);
}
