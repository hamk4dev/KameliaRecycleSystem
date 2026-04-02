using System.Security.Cryptography;
using System.Text;

namespace KameliaRecycleSystem.Security.Authentication;

public class LoginService
{
    public bool Authenticate(string username, string password, string expectedHash)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        return Hash(password) == expectedHash;
    }

    private static string Hash(string raw)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes);
    }
}
