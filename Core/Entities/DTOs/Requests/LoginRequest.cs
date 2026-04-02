using System.ComponentModel.DataAnnotations;

namespace KameliaRecycleSystem.Core.DTOs.Requests
{
    /// <summary>
    /// Data transfer object for user login authentication
    /// Used in: LoginForm.cs → LoginService.cs → Authentication Flow
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username wajib diisi")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username harus antara 3-50 karakter")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password wajib diisi")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password minimal 6 karakter")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public LoginRequest()
        {
            // Default constructor
        }

        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}