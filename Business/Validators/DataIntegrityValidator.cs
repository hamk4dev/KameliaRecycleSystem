namespace KameliaRecycleSystem.Business.Validators;

public class DataIntegrityValidator
{
    public bool ValidateKey(string value) => !string.IsNullOrWhiteSpace(value) && value.Trim().Length >= 3;
}
