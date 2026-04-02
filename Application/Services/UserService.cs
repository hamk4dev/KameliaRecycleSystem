using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KameliaRecycleSystem.Core.Interfaces;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.Enums;
using KameliaRecycleSystem.Infrastructure.Security.Authentication;
using KameliaRecycleSystem.Core.Exceptions;
using KameliaRecycleSystem.Shared.Constants;

namespace KameliaRecycleSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher();
        }

        // ===== USER MANAGEMENT OPERATIONS =====

        public async Task<UserAccount> CreateUserAsync(string username, string password, string email, UserRole role, int? wargaId = null)
        {
            // Validasi input
            if (string.IsNullOrWhiteSpace(username))
                throw new ValidationException("Username", "Username wajib diisi");
                
            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("Password", "Password wajib diisi");

            // Validasi business rules
            await ValidateUserCreationAsync(username, email);

            // Hash password
            var passwordHash = _passwordHasher.HashPassword(password);

            // Buat user entity
            var user = new UserAccount(username, passwordHash, role)
            {
                Email = email,
                WargaId = wargaId,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            // Simpan ke database
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<UserAccount> UpdateUserAsync(int userId, string email, UserRole role, int? wargaId, bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new DomainException("USER_NOT_FOUND", $"User dengan ID {userId} tidak ditemukan");

            // Validasi email uniqueness jika berubah
            if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                if (await _userRepository.EmailExistsAsync(email))
                    throw new ValidationException("Email", "Email sudah digunakan oleh user lain");
            }

            // Update properties
            user.Email = email;
            user.Role = role;
            user.WargaId = wargaId;
            user.IsActive = isActive;
            user.ModifiedDate = DateTime.Now;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            // Soft delete
            user.IsActive = false;
            user.ModifiedDate = DateTime.Now;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        // ===== PASSWORD MANAGEMENT =====

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new DomainException("USER_NOT_FOUND", "User tidak ditemukan");

            // Validasi password strength
            if (newPassword.Length < SecurityConstants.PasswordMinLength)
                throw new ValidationException("Password", SecurityConstants.FormatPasswordRequirementMessage());

            // Hash password baru
            user.PasswordHash = _passwordHasher.HashPassword(newPassword);
            user.ModifiedDate = DateTime.Now;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResetPasswordAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new DomainException("USER_NOT_FOUND", "User tidak ditemukan");

            // Generate temporary password (bisa di-enhance nanti)
            var tempPassword = GenerateTemporaryPassword();
            user.PasswordHash = _passwordHasher.HashPassword(tempPassword);
            user.ModifiedDate = DateTime.Now;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            // TODO: Kirim email dengan temporary password
            return true;
        }

        // ===== USER QUERIES =====

        public async Task<UserAccount> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<UserAccount> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<UserAccount> GetUserWithWargaAsync(int userId)
        {
            return await _userRepository.GetUserWithWargaAsync(userId);
        }

        public async Task<IEnumerable<UserAccount>> GetUsersByRoleAsync(UserRole role)
        {
            return await _userRepository.GetByRoleAsync(role.ToString());
        }

        public async Task<IEnumerable<UserAccount>> SearchUsersAsync(string searchTerm)
        {
            return await _userRepository.SearchUsersAsync(searchTerm);
        }

        public async Task<IEnumerable<UserAccount>> GetActiveUsersAsync()
        {
            return await _userRepository.GetActiveUsersAsync();
        }

        public async Task<IEnumerable<UserAccount>> GetInactiveUsersAsync()
        {
            return await _userRepository.GetInactiveUsersAsync();
        }

        // ===== ROLE MANAGEMENT =====

        public async Task<bool> UpdateUserRoleAsync(int userId, UserRole newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.Role = newRole;
            user.ModifiedDate = DateTime.Now;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetUserCountByRoleAsync(UserRole role)
        {
            return await _userRepository.GetUserCountByRoleAsync(role.ToString());
        }

        // ===== VALIDATION METHODS =====

        private async Task ValidateUserCreationAsync(string username, string email)
        {
            // Validasi username uniqueness
            if (await _userRepository.UsernameExistsAsync(username))
                throw new ValidationException("Username", "Username sudah digunakan");

            // Validasi email uniqueness
            if (!string.IsNullOrWhiteSpace(email) && await _userRepository.EmailExistsAsync(email))
                throw new ValidationException("Email", "Email sudah digunakan");

            // Validasi password policy
            // (Bisa ditambahkan validasi complexity nanti di UserValidator)
        }

        private string GenerateTemporaryPassword()
        {
            // Simple temporary password generator
            // Bisa di-enhance nanti dengan pattern yang lebih complex
            return "Temp123";
        }
    }
}