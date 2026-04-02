using System.Collections.Generic;
using System.Threading.Tasks;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.Interfaces
{
    public interface IUserService
    {
        // User Management
        Task<UserAccount> CreateUserAsync(string username, string password, string email, UserRole role, int? wargaId = null);
        Task<UserAccount> UpdateUserAsync(int userId, string email, UserRole role, int? wargaId, bool isActive);
        Task<bool> DeleteUserAsync(int userId);

        // Password Management
        Task<bool> ChangePasswordAsync(int userId, string newPassword);
        Task<bool> ResetPasswordAsync(int userId);

        // User Queries
        Task<UserAccount> GetUserByIdAsync(int userId);
        Task<UserAccount> GetUserByUsernameAsync(string username);
        Task<UserAccount> GetUserWithWargaAsync(int userId);
        Task<IEnumerable<UserAccount>> GetUsersByRoleAsync(UserRole role);
        Task<IEnumerable<UserAccount>> SearchUsersAsync(string searchTerm);
        Task<IEnumerable<UserAccount>> GetActiveUsersAsync();
        Task<IEnumerable<UserAccount>> GetInactiveUsersAsync();

        // Role Management
        Task<bool> UpdateUserRoleAsync(int userId, UserRole newRole);
        Task<int> GetUserCountByRoleAsync(UserRole role);
    }
}