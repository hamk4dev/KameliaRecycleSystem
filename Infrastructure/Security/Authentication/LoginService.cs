using KameliaRecycleSystem.Core.Interfaces;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.DTOs.Requests;
using KameliaRecycleSystem.Core.DTOs.Responses;
using KameliaRecycleSystem.Shared.Constants;
using KameliaRecycleSystem.Core.Exceptions;

namespace KameliaRecycleSystem.Infrastructure.Security.Authentication
{
    public class LoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtService _jwtService;

        public LoginService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher();
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            // Validasi input
            if (string.IsNullOrWhiteSpace(request.Username))
                return LoginResponse.Failure("Username wajib diisi");
                
            if (string.IsNullOrWhiteSpace(request.Password))
                return LoginResponse.Failure("Password wajib diisi");

            try
            {
                // Cari user by username
                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null)
                    return LoginResponse.Failure(SecurityConstants.MessageInvalidCredentials);

                // Cek status akun menggunakan business logic dari UserAccount
                if (!user.CanLogin())
                {
                    if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
                    {
                        var minutesLeft = (int)(user.LockoutEnd.Value - DateTime.Now).TotalMinutes;
                        return LoginResponse.Failure(SecurityConstants.FormatLockoutMessage(minutesLeft));
                    }
                    return LoginResponse.Failure("Akun tidak aktif");
                }

                // Verifikasi password
                if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                {
                    await HandleFailedLoginAttempt(user);
                    return LoginResponse.Failure(SecurityConstants.MessageInvalidCredentials);
                }

                // Login berhasil - reset attempts & update last login
                await _userRepository.ResetFailedLoginCountAsync(user.Id);
                await _userRepository.UpdateLastLoginAsync(user.Id);
                
                // Update entity untuk konsistensi
                user.UpdateLastLogin();

                // Generate JWT token menggunakan JwtService yang sudah ada
                var token = _jwtService.GenerateToken(user);
                var tokenExpiry = DateTime.UtcNow.AddMinutes(SecurityConstants.JwtExpirationMinutes);

                // Prepare display name dengan data warga jika ada
                string displayName = user.Username;
                int? wargaId = user.WargaId;
                
                if (user.WargaId.HasValue)
                {
                    var userWithWarga = await _userRepository.GetUserWithWargaAsync(user.Id);
                    if (userWithWarga?.Warga != null)
                    {
                        displayName = userWithWarga.Warga.Nama ?? user.Username;
                        wargaId = userWithWarga.Warga.Id;
                    }
                }

                return LoginResponse.Success(
                    token: token,
                    tokenExpiry: tokenExpiry,
                    userId: user.Id,
                    username: user.Username,
                    role: user.Role,
                    displayName: displayName,
                    email: user.Email,
                    lastLogin: user.LastLogin,
                    wargaId: wargaId
                );
            }
            catch (Exception)
            {
                // Log error (bisa ditambahkan logging nanti)
                return LoginResponse.Failure("Terjadi kesalahan sistem saat proses login");
            }
        }

        private async Task HandleFailedLoginAttempt(UserAccount user)
        {
            var failedCount = await _userRepository.GetFailedLoginCountAsync(user.Id) + 1;
            
            // Update failed count di repository
            await _userRepository.UpdateFailedLoginCount(user.Id, failedCount);
            
            // Jika melebihi max attempts, lock account menggunakan repository method
            if (failedCount >= SecurityConstants.MaxLoginAttempts)
            {
                var lockoutEnd = DateTime.Now.AddMinutes(SecurityConstants.LockoutDurationMinutes);
                await _userRepository.LockUserAccountAsync(user.Id, lockoutEnd);
            }
        }

        public bool ValidateToken(string token)
        {
            return _jwtService.IsTokenValid(token);
        }

        public async Task<bool> IsUserActiveAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user?.IsActive == true && user.CanLogin();
        }
    }
}
