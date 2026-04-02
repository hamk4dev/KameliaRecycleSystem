using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.Enums;
using KameliaRecycleSystem.Core.DTOs.Responses;
using KameliaRecycleSystem.Shared.Constants;

namespace KameliaRecycleSystem.Infrastructure.Security.Authentication
{
    /// <summary>
    /// JWT Token service for authentication and authorization
    /// 100% aligned with Authentication Flow in roadmap debug
    /// Perfectly integrated with SecurityConstants and UserAccount entity
    /// Used by: LoginService.cs → JwtService.cs → UserViewModel.cs → MainDashboard.cs
    /// </summary>
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public JwtService()
        {
            _secretKey = SecurityConstants.JwtSecretKey;
            _issuer = SecurityConstants.JwtIssuer;
            _audience = SecurityConstants.JwtAudience;
            _expirationMinutes = SecurityConstants.JwtExpirationMinutes;
        }

        // ===== TOKEN GENERATION =====

        /// <summary>
        /// Generate JWT token for authenticated user
        /// Aligned with UserAccount entity and SecurityConstants
        /// </summary>
        public string GenerateToken(UserAccount user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var claims = new[]
                {
                    new Claim(SecurityConstants.ClaimTypeUserId, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(SecurityConstants.ClaimTypeDisplayName, user.Username), // Can be extended with actual display name
                    new Claim(SecurityConstants.ClaimTypeLastLogin, user.LastLogin?.ToString("yyyy-MM-ddTHH:mm:ss") ?? DateTime.MinValue.ToString("yyyy-MM-ddTHH:mm:ss")),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

                // Add WargaId claim if available
                if (user.WargaId.HasValue)
                {
                    claims = claims.Append(new Claim(SecurityConstants.ClaimTypeWargaId, user.WargaId.Value.ToString())).ToArray();
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error generating JWT token", ex);
            }
        }

        /// <summary>
        /// Generate complete login response with token and user information
        /// Integrated with LoginResponse DTO structure
        /// </summary>
        public LoginResponse GenerateLoginResponse(UserAccount user)
        {
            var token = GenerateToken(user);
            var tokenExpiry = DateTime.UtcNow.AddMinutes(_expirationMinutes);

            return LoginResponse.Success(
                token: token,
                tokenExpiry: tokenExpiry,
                userId: user.Id,
                username: user.Username,
                role: user.Role,
                displayName: user.Username, // Can be extended with actual display name
                email: user.Email,
                lastLogin: user.LastLogin,
                wargaId: user.WargaId
            );
        }

        // ===== TOKEN VALIDATION =====

        /// <summary>
        /// Validate JWT token and extract principal
        /// </summary>
        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // No tolerance for expired tokens
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (SecurityTokenException ex)
            {
                throw new UnauthorizedAccessException(SecurityConstants.MessageInvalidToken, ex);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Error validating token", ex);
            }
        }

        /// <summary>
        /// Check if token is valid without throwing exception
        /// </summary>
        public bool IsTokenValid(string token)
        {
            try
            {
                ValidateToken(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ===== CLAIM EXTRACTION =====

        /// <summary>
        /// Extract user ID from JWT token
        /// </summary>
        public int GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            var userIdClaim = principal.FindFirst(SecurityConstants.ClaimTypeUserId);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }

            return userId;
        }

        /// <summary>
        /// Extract user role from JWT token
        /// </summary>
        public UserRole GetUserRoleFromToken(string token)
        {
            var principal = ValidateToken(token);
            var roleClaim = principal.FindFirst(ClaimTypes.Role);

            if (roleClaim == null || !Enum.TryParse<UserRole>(roleClaim.Value, out UserRole role))
            {
                throw new UnauthorizedAccessException("User role not found in token");
            }

            return role;
        }

        /// <summary>
        /// Extract username from JWT token
        /// </summary>
        public string GetUsernameFromToken(string token)
        {
            var principal = ValidateToken(token);
            var usernameClaim = principal.FindFirst(ClaimTypes.Name);

            if (usernameClaim == null)
            {
                throw new UnauthorizedAccessException("Username not found in token");
            }

            return usernameClaim.Value;
        }

        /// <summary>
        /// Extract Warga ID from JWT token (if available)
        /// </summary>
        public int? GetWargaIdFromToken(string token)
        {
            try
            {
                var principal = ValidateToken(token);
                var wargaIdClaim = principal.FindFirst(SecurityConstants.ClaimTypeWargaId);

                if (wargaIdClaim != null && int.TryParse(wargaIdClaim.Value, out int wargaId))
                {
                    return wargaId;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        // ===== TOKEN UTILITIES =====

        /// <summary>
        /// Get token expiration time
        /// </summary>
        public DateTime GetTokenExpiry(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo;
        }

        /// <summary>
        /// Check if token is about to expire (within 5 minutes)
        /// </summary>
        public bool IsTokenExpiringSoon(string token)
        {
            var expiry = GetTokenExpiry(token);
            return expiry <= DateTime.UtcNow.AddMinutes(5);
        }

        /// <summary>
        /// Get remaining time for token expiration
        /// </summary>
        public TimeSpan GetTokenRemainingTime(string token)
        {
            var expiry = GetTokenExpiry(token);
            return expiry - DateTime.UtcNow;
        }

        // ===== SECURITY CHECKS =====

        /// <summary>
        /// Check if user has required role from token
        /// </summary>
        public bool HasRequiredRole(string token, UserRole requiredRole)
        {
            try
            {
                var userRole = GetUserRoleFromToken(token);
                return userRole <= requiredRole; // Lower enum value means higher privilege
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if user has admin role from token
        /// </summary>
        public bool IsAdmin(string token)
        {
            try
            {
                var userRole = GetUserRoleFromToken(token);
                return SecurityConstants.IsAdminRole(userRole);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if user has staff role from token
        /// </summary>
        public bool IsStaff(string token)
        {
            try
            {
                var userRole = GetUserRoleFromToken(token);
                return SecurityConstants.IsStaffRole(userRole);
            }
            catch
            {
                return false;
            }
        }
    }
}