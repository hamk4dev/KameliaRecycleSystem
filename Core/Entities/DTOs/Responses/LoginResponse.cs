using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.DTOs.Responses
{
    /// <summary>
    /// Data transfer object for login authentication response
    /// Used in: LoginService.cs → JwtService.cs → UserViewModel.cs → MainDashboard.cs
    /// </summary>
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiry { get; set; }
        
        // User information for UI display
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string DisplayName { get; set; }
        
        // Additional user context
        public DateTime? LastLogin { get; set; }
        public bool IsFirstLogin { get; set; }
        public int? WargaId { get; set; } // Optional link to resident data

        public LoginResponse()
        {
        }

        public LoginResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public LoginResponse(bool isSuccess, string message, string token, DateTime tokenExpiry, 
                           int userId, string username, UserRole role, string displayName)
        {
            IsSuccess = isSuccess;
            Message = message;
            Token = token;
            TokenExpiry = tokenExpiry;
            UserId = userId;
            Username = username;
            Role = role;
            DisplayName = displayName;
        }

        // Success static method
        public static LoginResponse Success(string token, DateTime tokenExpiry, int userId, 
                                          string username, UserRole role, string displayName, 
                                          string email = null, DateTime? lastLogin = null, 
                                          int? wargaId = null)
        {
            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Login berhasil",
                Token = token,
                TokenExpiry = tokenExpiry,
                UserId = userId,
                Username = username,
                Email = email,
                Role = role,
                DisplayName = displayName,
                LastLogin = lastLogin,
                IsFirstLogin = lastLogin == null,
                WargaId = wargaId
            };
        }

        // Failure static method
        public static LoginResponse Failure(string message)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = message
            };
        }
    }
}