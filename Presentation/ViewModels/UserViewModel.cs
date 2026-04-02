using System;
using KameliaRecycleSystem.Core.DTOs.Responses;
using KameliaRecycleSystem.Core.Enums;
using KameliaRecycleSystem.Infrastructure.Security.Authentication;

namespace KameliaRecycleSystem.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for user session management and UI data binding
    /// Perfectly integrated with LoginResponse and JwtService
    /// Used by: LoginForm.cs → MainDashboard.cs → All authenticated forms
    /// </summary>
    public class UserViewModel
    {
        private readonly JwtService _jwtService;

        // ===== USER PROPERTIES (From LoginResponse) =====
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiry { get; set; }
        public int? WargaId { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsFirstLogin { get; set; }

        // ===== UI STATE PROPERTIES =====
        public bool IsAuthenticated => !string.IsNullOrEmpty(Token) && TokenExpiry > DateTime.UtcNow;
        public bool IsAdmin => Role == UserRole.Admin;
        public bool IsStaff => Role == UserRole.Pegawai || Role == UserRole.Admin;
        public bool IsWarga => Role == UserRole.Warga;
        public bool HasWargaLink => WargaId.HasValue;
        public string RoleDisplayName => GetRoleDisplayName();
        public TimeSpan TokenRemainingTime => TokenExpiry - DateTime.UtcNow;

        public UserViewModel(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        // ===== CONSTRUCTORS =====

        public UserViewModel()
        {
            // Default constructor for design time
        }

        /// <summary>
        /// Create UserViewModel from successful login response
        /// Integrated with LoginResponse structure
        /// </summary>
        public UserViewModel(LoginResponse loginResponse, JwtService jwtService)
        {
            _jwtService = jwtService;
            UpdateFromLoginResponse(loginResponse);
        }

        // ===== UPDATE METHODS =====

        /// <summary>
        /// Update ViewModel from LoginResponse after successful authentication
        /// </summary>
        public void UpdateFromLoginResponse(LoginResponse loginResponse)
        {
            if (loginResponse == null || !loginResponse.IsSuccess)
                throw new ArgumentException("Invalid login response");

            UserId = loginResponse.UserId;
            Username = loginResponse.Username;
            Email = loginResponse.Email;
            Role = loginResponse.Role;
            DisplayName = loginResponse.DisplayName;
            Token = loginResponse.Token;
            TokenExpiry = loginResponse.TokenExpiry;
            WargaId = loginResponse.WargaId;
            LastLogin = loginResponse.LastLogin;
            IsFirstLogin = loginResponse.IsFirstLogin;
        }

        /// <summary>
        /// Clear user data on logout
        /// </summary>
        public void Clear()
        {
            UserId = 0;
            Username = string.Empty;
            Email = string.Empty;
            Role = UserRole.Warga; // Default role
            DisplayName = string.Empty;
            Token = string.Empty;
            TokenExpiry = DateTime.MinValue;
            WargaId = null;
            LastLogin = null;
            IsFirstLogin = false;
        }

        // ===== TOKEN MANAGEMENT =====

        /// <summary>
        /// Validate JWT token using JwtService
        /// </summary>
        public bool IsTokenValid()
        {
            if (string.IsNullOrEmpty(Token)) return false;
            
            try
            {
                return _jwtService?.IsTokenValid(Token) == true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if token is expiring soon (within 5 minutes)
        /// </summary>
        public bool IsTokenExpiringSoon()
        {
            if (string.IsNullOrEmpty(Token)) return true;
            
            try
            {
                return _jwtService?.IsTokenExpiringSoon(Token) == true;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Get remaining time until token expiry
        /// </summary>
        public TimeSpan GetTokenRemainingTime()
        {
            if (string.IsNullOrEmpty(Token)) return TimeSpan.Zero;
            
            try
            {
                return _jwtService?.GetTokenRemainingTime(Token) ?? TimeSpan.Zero;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        // ===== ROLE-BASED SECURITY =====

        /// <summary>
        /// Check if user has required role permission
        /// Uses JwtService for token-based role validation
        /// </summary>
        public bool HasRequiredRole(UserRole requiredRole)
        {
            if (string.IsNullOrEmpty(Token)) return false;
            
            try
            {
                return _jwtService?.HasRequiredRole(Token, requiredRole) == true;
            }
            catch
            {
                return Role <= requiredRole; // Fallback to ViewModel role check
            }
        }

        /// <summary>
        /// Check if user can access admin features
        /// </summary>
        public bool CanAccessAdminFeatures()
        {
            return HasRequiredRole(UserRole.Admin);
        }

        /// <summary>
        /// Check if user can access staff features
        /// </summary>
        public bool CanAccessStaffFeatures()
        {
            return HasRequiredRole(UserRole.Pegawai);
        }

        /// <summary>
        /// Check if user can manage financial data
        /// </summary>
        public bool CanManageFinancials()
        {
            return HasRequiredRole(UserRole.Pegawai); // Staff and above
        }

        /// <summary>
        /// Check if user can manage user accounts
        /// </summary>
        public bool CanManageUsers()
        {
            return HasRequiredRole(UserRole.Admin); // Admin only
        }

        // ===== UTILITY METHODS =====

        private string GetRoleDisplayName()
        {
            return Role switch
            {
                UserRole.Admin => "Administrator",
                UserRole.Pegawai => "Staff",
                UserRole.Warga => "Warga",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Get user information for display purposes
        /// </summary>
        public string GetUserInfo()
        {
            return $"{DisplayName} ({Username}) - {RoleDisplayName}";
        }

        /// <summary>
        /// Check if user needs to change password (first login)
        /// </summary>
        public bool ShouldChangePassword()
        {
            return IsFirstLogin;
        }

        /// <summary>
        /// Validate if user session is still active and valid
        /// </summary>
        public bool IsSessionValid()
        {
            return IsAuthenticated && IsTokenValid() && !IsTokenExpiringSoon();
        }

        // ===== OVERRIDES =====

        public override string ToString()
        {
            return $"User: {DisplayName}, Role: {RoleDisplayName}, Authenticated: {IsAuthenticated}";
        }
    }
}
