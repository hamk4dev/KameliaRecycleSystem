using System.Text;

namespace KameliaRecycleSystem.Security.Encryption;

public class DataEncryptor
{
    public string Encrypt(string plainText)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText ?? string.Empty));
    }

    public string Decrypt(string cipherText)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(cipherText ?? string.Empty));
    }
}
