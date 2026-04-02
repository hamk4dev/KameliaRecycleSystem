using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Shared.Constants
{
    /// <summary>
    /// Security-related constants for authentication, authorization, and encryption
    /// Used in: JwtService.cs, PasswordHasher.cs, PermissionService.cs, and all security components
    /// </summary>
    public static class SecurityConstants
    {
        // ===== JWT TOKEN CONFIGURATION =====
        public const string JwtSecretKey = "KameliaRecycleSystem-SuperSecretKey-2024-MinLength-256Bits-For-Security";
        public const string JwtIssuer = "KameliaRecycleSystem";
        public const string JwtAudience = "KameliaRecycleSystem-Users";
        public const int JwtExpirationMinutes = 120; // 2 hours
        public const int JwtRefreshExpirationDays = 7; // 7 days

        // ===== PASSWORD POLICY =====
        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 100;
        public const int BCrypt_WORK_FACTOR = 11;
        public static readonly bool PasswordRequireDigit = true;
        public static readonly bool PasswordRequireLowercase = true;
        public static readonly bool PasswordRequireUppercase = false;
        public static readonly bool PasswordRequireNonAlphanumeric = false;
        public const int PasswordRequiredUniqueChars = 1;

        // ===== SECURITY SETTINGS =====
        public const int MaxLoginAttempts = 5;
        public const int LockoutDurationMinutes = 30;
        public const int PasswordResetTokenExpirationHours = 24;
        public const int SessionTimeoutMinutes = 30;

        // ===== ENCRYPTION SETTINGS =====
        public const string EncryptionKey = "Kamelia-256Bit-Encryption-Key-For-Data-Security-2024";
        public const string EncryptionIV = "Kamelia-128Bit-IV-2024";
        public const int SaltSize = 16; // 128 bit
        public const int KeySize = 32; // 256 bit
        public const int Iterations = 10000;

        // ===== ROLE-BASED SECURITY =====
        public static readonly UserRole[] AdminRoles = 
        { 
            UserRole.SuperAdmin, 
            UserRole.Admin 
        };

        public static readonly UserRole[] StaffRoles = 
        { 
            UserRole.SuperAdmin, 
            UserRole.Admin, 
            UserRole.Pegawai 
        };

        public static readonly UserRole[] AllRoles = 
        { 
            UserRole.SuperAdmin, 
            UserRole.Admin, 
            UserRole.Pegawai, 
            UserRole.Warga 
        };

        // ===== CLAIM TYPES =====
        public const string ClaimTypeUserId = "user_id";
        public const string ClaimTypeWargaId = "warga_id";
        public const string ClaimTypeDisplayName = "display_name";
        public const string ClaimTypeEmail = "email";
        public const string ClaimTypeLastLogin = "last_login";

        // ===== PERMISSION POLICIES =====
        public const string PolicySuperAdmin = "RequireSuperAdmin";
        public const string PolicyAdmin = "RequireAdmin";
        public const string PolicyStaff = "RequireStaff";
        public const string PolicyWarga = "RequireWarga";
        public const string PolicyCanManageUsers = "CanManageUsers";
        public const string PolicyCanApproveTransactions = "CanApproveTransactions";
        public const string PolicyCanManageWarga = "CanManageWarga";
        public const string PolicyCanViewReports = "CanViewReports";

        // ===== SECURITY HEADERS =====
        public const string ApiKeyHeader = "X-API-Key";
        public const string AuthorizationHeader = "Authorization";
        public const string TokenType = "Bearer";

        // ===== DEFAULT VALUES =====
        public const UserRole DefaultNewUserRole = UserRole.Warga;
        public const UserRole MinimumRoleForApproval = UserRole.Admin;
        public const string DefaultAdminUsername = "admin";
        public const string DefaultAdminPassword = "Admin123";

        // ===== SECURITY MESSAGES =====
        public const string MessageInvalidCredentials = "Username atau password salah";
        public const string MessageAccountLocked = "Akun terkunci. Silakan coba lagi setelah {0} menit";
        public const string MessageTokenExpired = "Token telah kadaluarsa";
        public const string MessageUnauthorized = "Anda tidak memiliki izin untuk mengakses resource ini";
        public const string MessageInvalidToken = "Token tidak valid";
        public const string MessagePasswordTooWeak = "Password terlalu lemah. Minimal {0} karakter dengan kombinasi huruf dan angka";

        // ===== SECURITY PATHS =====
        public const string LoginPath = "/auth/login";
        public const string AccessDeniedPath = "/auth/access-denied";
        public const string LogoutPath = "/auth/logout";
        public const string PasswordResetPath = "/auth/reset-password";

        // ===== SECURITY LOGGING =====
        public const string SecurityLogCategory = "Security";
        public const string AuthenticationLogCategory = "Authentication";
        public const string AuthorizationLogCategory = "Authorization";

        // ===== VALIDATION CONSTANTS =====
        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 50;
        public const int EmailMaxLength = 100;
        public const int DisplayNameMaxLength = 100;

        // ===== METHODS FOR BUSINESS LOGIC =====
        public static bool IsAdminRole(UserRole role)
        {
            return role == UserRole.SuperAdmin || role == UserRole.Admin;
        }

        public static bool IsStaffRole(UserRole role)
        {
            return role == UserRole.SuperAdmin || role == UserRole.Admin || role == UserRole.Pegawai;
        }

        public static bool CanManageUsers(UserRole role)
        {
            return IsAdminRole(role);
        }

        public static bool CanApproveTransactions(UserRole role)
        {
            return IsAdminRole(role);
        }

        public static bool CanManageWarga(UserRole role)
        {
            return IsStaffRole(role);
        }

        public static bool CanViewSensitiveData(UserRole role)
        {
            return IsStaffRole(role);
        }

        public static string GetRoleDisplayName(UserRole role)
        {
            return role switch
            {
                UserRole.SuperAdmin => "Super Administrator",
                UserRole.Admin => "Administrator",
                UserRole.Pegawai => "Staff Operasional",
                UserRole.Warga => "Warga",
                _ => role.ToString()
            };
        }

        public static string FormatLockoutMessage(int minutesRemaining)
        {
            return string.Format(MessageAccountLocked, minutesRemaining);
        }

        public static string FormatPasswordRequirementMessage()
        {
            var requirements = new List<string>();
            
            if (PasswordRequireDigit)
                requirements.Add("angka");
            if (PasswordRequireLowercase)
                requirements.Add("huruf kecil");
            if (PasswordRequireUppercase)
                requirements.Add("huruf besar");
            if (PasswordRequireNonAlphanumeric)
                requirements.Add("karakter khusus");
                
            var requirementText = requirements.Any() 
                ? $" dengan kombinasi {string.Join(" dan ", requirements)}" 
                : "";
                
            return $"Password minimal {PasswordMinLength} karakter{requirementText}";
        }
    }
}
