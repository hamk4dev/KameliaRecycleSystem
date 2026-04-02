using System;
using System.Text.RegularExpressions;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.Enums;
using KameliaRecycleSystem.Core.Exceptions;
using KameliaRecycleSystem.Shared.Constants;

namespace KameliaRecycleSystem.Application.Validators
{
    public class UserValidator
    {
        private readonly Regex _emailRegex;
        private readonly Regex _usernameRegex;

        public UserValidator()
        {
            _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _usernameRegex = new Regex(@"^[a-zA-Z0-9_]+$", RegexOptions.Compiled);
        }

        // ===== COMPREHENSIVE VALIDATION METHODS =====

        public void ValidateCreateUser(string username, string password, string email, UserRole role)
        {
            var errors = new Dictionary<string, string[]>();

            // Validate username
            var usernameErrors = ValidateUsername(username);
            if (usernameErrors.Any())
                errors.Add("Username", usernameErrors);

            // Validate password
            var passwordErrors = ValidatePassword(password);
            if (passwordErrors.Any())
                errors.Add("Password", passwordErrors);

            // Validate email
            var emailErrors = ValidateEmail(email);
            if (emailErrors.Any())
                errors.Add("Email", emailErrors);

            // Validate role
            var roleErrors = ValidateRole(role);
            if (roleErrors.Any())
                errors.Add("Role", roleErrors);

            if (errors.Any())
                throw new ValidationException(errors);
        }

        public void ValidateUpdateUser(string email, UserRole role, bool isActive)
        {
            var errors = new Dictionary<string, string[]>();

            // Validate email
            var emailErrors = ValidateEmail(email);
            if (emailErrors.Any())
                errors.Add("Email", emailErrors);

            // Validate role
            var roleErrors = ValidateRole(role);
            if (roleErrors.Any())
                errors.Add("Role", roleErrors);

            if (errors.Any())
                throw new ValidationException(errors);
        }

        public void ValidateUserEntity(UserAccount user)
        {
            if (user == null)
                throw new ValidationException("User", "User account cannot be null");

            var errors = new Dictionary<string, string[]>();

            // Validate entity properties
            var usernameErrors = ValidateUsername(user.Username);
            if (usernameErrors.Any())
                errors.Add("Username", usernameErrors);

            if (!string.IsNullOrEmpty(user.Email))
            {
                var emailErrors = ValidateEmail(user.Email);
                if (emailErrors.Any())
                    errors.Add("Email", emailErrors);
            }

            var roleErrors = ValidateRole(user.Role);
            if (roleErrors.Any())
                errors.Add("Role", roleErrors);

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ===== INDIVIDUAL PROPERTY VALIDATORS =====

        public string[] ValidateUsername(string username)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Username wajib diisi");
                return errors.ToArray();
            }

            if (username.Length < SecurityConstants.UsernameMinLength)
                errors.Add($"Username minimal {SecurityConstants.UsernameMinLength} karakter");

            if (username.Length > SecurityConstants.UsernameMaxLength)
                errors.Add($"Username maksimal {SecurityConstants.UsernameMaxLength} karakter");

            if (!_usernameRegex.IsMatch(username))
                errors.Add("Username hanya boleh mengandung huruf, angka, dan underscore");

            return errors.ToArray();
        }

        public string[] ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password wajib diisi");
                return errors.ToArray();
            }

            if (password.Length < SecurityConstants.PasswordMinLength)
                errors.Add($"Password minimal {SecurityConstants.PasswordMinLength} karakter");

            if (password.Length > SecurityConstants.PasswordMaxLength)
                errors.Add($"Password maksimal {SecurityConstants.PasswordMaxLength} karakter");

            // Password complexity validation
            if (SecurityConstants.PasswordRequireDigit && !password.Any(char.IsDigit))
                errors.Add("Password harus mengandung angka");

            if (SecurityConstants.PasswordRequireLowercase && !password.Any(char.IsLower))
                errors.Add("Password harus mengandung huruf kecil");

            if (SecurityConstants.PasswordRequireUppercase && !password.Any(char.IsUpper))
                errors.Add("Password harus mengandung huruf besar");

            if (SecurityConstants.PasswordRequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                errors.Add("Password harus mengandung karakter khusus");

            return errors.ToArray();
        }

        public string[] ValidateEmail(string email)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(email))
            {
                // Email is optional in some cases, so no error for empty
                return errors.ToArray();
            }

            if (email.Length > SecurityConstants.EmailMaxLength)
                errors.Add($"Email maksimal {SecurityConstants.EmailMaxLength} karakter");

            if (!_emailRegex.IsMatch(email))
                errors.Add("Format email tidak valid");

            return errors.ToArray();
        }

        public string[] ValidateRole(UserRole role)
        {
            var errors = new List<string>();

            if (!Enum.IsDefined(typeof(UserRole), role))
                errors.Add("Role tidak valid");

            return errors.ToArray();
        }

        public string[] ValidatePasswordChange(string currentPassword, string newPassword)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(currentPassword))
                errors.Add("Password saat ini wajib diisi");

            if (string.IsNullOrWhiteSpace(newPassword))
                errors.Add("Password baru wajib diisi");
            else
                errors.AddRange(ValidatePassword(newPassword));

            if (currentPassword == newPassword)
                errors.Add("Password baru harus berbeda dengan password saat ini");

            return errors.ToArray();
        }

        // ===== QUICK VALIDATION METHODS =====

        public bool IsValidUsername(string username)
        {
            return !ValidateUsername(username).Any();
        }

        public bool IsValidPassword(string password)
        {
            return !ValidatePassword(password).Any();
        }

        public bool IsValidEmail(string email)
        {
            return !ValidateEmail(email).Any();
        }

        public bool IsValidRole(UserRole role)
        {
            return !ValidateRole(role).Any();
        }

        // ===== BUSINESS RULE VALIDATORS =====

        public void ValidateUserCanLogin(UserAccount user)
        {
            if (user == null)
                throw new ValidationException("User", "User tidak ditemukan");

            if (!user.IsActive)
                throw new ValidationException("User", "Akun tidak aktif");

            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
            {
                var minutesLeft = (int)(user.LockoutEnd.Value - DateTime.Now).TotalMinutes;
                throw new ValidationException("User", SecurityConstants.FormatLockoutMessage(minutesLeft));
            }
        }

        public void ValidatePasswordReset(string newPassword, string confirmPassword)
        {
            var errors = new Dictionary<string, string[]>();

            var passwordErrors = ValidatePassword(newPassword);
            if (passwordErrors.Any())
                errors.Add("NewPassword", passwordErrors);

            if (newPassword != confirmPassword)
                errors.Add("ConfirmPassword", new[] { "Konfirmasi password tidak sesuai" });

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ===== UTILITY METHODS =====

        public string GetPasswordRequirementDescription()
        {
            return SecurityConstants.FormatPasswordRequirementMessage();
        }

        public string GetUsernameRequirementDescription()
        {
            return $"Username minimal {SecurityConstants.UsernameMinLength} karakter, maksimal {SecurityConstants.UsernameMaxLength} karakter, hanya boleh mengandung huruf, angka, dan underscore";
        }
    }
}