using KameliaRecycleSystem.Shared.Constants;

namespace KameliaRecycleSystem.Infrastructure.Security.Authentication
{
    /// <summary>
    /// Service untuk hashing dan verifikasi password menggunakan BCrypt
    /// Integrated dengan SecurityConstants dan UserAccount entity
    /// </summary>
    public class PasswordHasher
    {
        /// <summary>
        /// Hash password menggunakan BCrypt dengan work factor dari SecurityConstants
        /// </summary>
        /// <param name="password">Password plaintext</param>
        /// <returns>Hashed password string</returns>
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, SecurityConstants.BCrypt_WORK_FACTOR);
        }
        
        /// <summary>
        /// Verifikasi password terhadap hash yang disimpan
        /// </summary>
        /// <param name="password">Password plaintext</param>
        /// <param name="hashedPassword">Hashed password dari database</param>
        /// <returns>True jika password valid</returns>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
