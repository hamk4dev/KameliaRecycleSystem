using System.Security.Cryptography;
using System.Text;

namespace KameliaRecycleSystem.Security.Authentication;

public class PasswordHasher
{
    public string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password ?? string.Empty));
        return Convert.ToHexString(bytes);
    }

    public bool Verify(string password, string hashedPassword)
    {
        return Hash(password) == hashedPassword;
    }
}
