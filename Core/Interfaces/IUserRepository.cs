using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KameliaRecycleSystem.Core.Entities;

namespace KameliaRecycleSystem.Core.Interfaces
{
    /// <summary>
    /// Repository interface for UserAccount data access operations
    /// Used in: LoginService.cs → IUserRepository → UserRepository.cs → AppDbContext.cs → UserAccount.cs
    /// </summary>
    public interface IUserRepository
    {
        // Basic CRUD Operations
        Task<UserAccount> GetByIdAsync(int id);
        Task<UserAccount> GetByUsernameAsync(string username);
        Task<UserAccount> GetByEmailAsync(string email);
        Task<IEnumerable<UserAccount>> GetAllAsync();
        Task AddAsync(UserAccount user);
        void Update(UserAccount user);
        Task DeleteAsync(int id);

        // Authentication Specific Operations
        Task<UserAccount> GetUserWithWargaAsync(int userId);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        
        // Role-based Operations
        Task<IEnumerable<UserAccount>> GetByRoleAsync(string role);
        Task<int> GetUserCountByRoleAsync(string role);
        
        // Search and Filter Operations
        Task<IEnumerable<UserAccount>> SearchUsersAsync(string searchTerm);
        Task<IEnumerable<UserAccount>> GetActiveUsersAsync();
        Task<IEnumerable<UserAccount>> GetInactiveUsersAsync();
        
        // Audit and Maintenance Operations
        Task UpdateLastLoginAsync(int userId);
        Task<int> GetFailedLoginCountAsync(int userId);
        Task ResetFailedLoginCountAsync(int userId);
        Task LockUserAccountAsync(int userId, DateTime lockoutEnd);
        Task UnlockUserAccountAsync(int userId);
        
        // ✅ TAMBAHKAN METHOD INI UNTUK LOGINSERVICE:
        Task UpdateFailedLoginCount(int userId, int failedCount);
        
        // Bulk Operations
        Task<int> SaveChangesAsync();
    }
}