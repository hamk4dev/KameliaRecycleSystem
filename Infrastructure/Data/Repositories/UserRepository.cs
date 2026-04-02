using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.Interfaces;
using KameliaRecycleSystem.Core.Exceptions;
using KameliaRecycleSystem.Infrastructure.Data;

namespace KameliaRecycleSystem.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for UserAccount data operations
    /// 100% aligned with Authentication Flow in roadmap debug
    /// Implements IUserRepository contract with BaseRepository foundation
    /// Perfectly integrated with existing entities and security flow
    /// </summary>
    public class UserRepository : BaseRepository<UserAccount>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        // ===== AUTHENTICATION SPECIFIC OPERATIONS =====

        public async Task<UserAccount> GetByUsernameAsync(string username)
        {
            try
            {
                return await _dbSet
                    .FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving user by username: {username}", ex);
            }
        }

        public async Task<UserAccount> GetByEmailAsync(string email)
        {
            try
            {
                return await _dbSet
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving user by email: {email}", ex);
            }
        }

        public async Task<UserAccount> GetUserWithWargaAsync(int userId)
        {
            try
            {
                return await _dbSet
                    .Include(u => u.Warga)
                    .FirstOrDefaultAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving user with warga data for ID: {userId}", ex);
            }
        }

        // ===== VALIDATION & EXISTENCE CHECKS =====

        public async Task<bool> UsernameExistsAsync(string username)
        {
            try
            {
                return await _dbSet.AnyAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error checking username existence: {username}", ex);
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                return await _dbSet.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error checking email existence: {email}", ex);
            }
        }

        // ===== ROLE-BASED OPERATIONS =====

        public async Task<IEnumerable<UserAccount>> GetByRoleAsync(string role)
        {
            try
            {
                return await _dbSet
                    .Where(u => u.Role.ToString() == role)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving users by role: {role}", ex);
            }
        }

        public async Task<int> GetUserCountByRoleAsync(string role)
        {
            try
            {
                return await _dbSet
                    .CountAsync(u => u.Role.ToString() == role);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error counting users by role: {role}", ex);
            }
        }

        // ===== SEARCH AND FILTER OPERATIONS =====

        public async Task<IEnumerable<UserAccount>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllAsync();

                return await _dbSet
                    .Where(u => u.Username.Contains(searchTerm) || 
                               u.Email.Contains(searchTerm) ||
                               u.Warga.Nama.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error searching users with term: {searchTerm}", ex);
            }
        }

        public async Task<IEnumerable<UserAccount>> GetActiveUsersAsync()
        {
            try
            {
                return await _dbSet
                    .Where(u => u.IsActive)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error retrieving active users", ex);
            }
        }

        public async Task<IEnumerable<UserAccount>> GetInactiveUsersAsync()
        {
            try
            {
                return await _dbSet
                    .Where(u => !u.IsActive)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error retrieving inactive users", ex);
            }
        }

        // ===== SECURITY & AUDIT OPERATIONS =====

        public async Task UpdateLastLoginAsync(int userId)
        {
            try
            {
                var user = await GetByIdAsync(userId);
                if (user != null)
                {
                    user.UpdateLastLogin();
                    Update(user);
                    await SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error updating last login for user ID: {userId}", ex);
            }
        }

        public async Task<int> GetFailedLoginCountAsync(int userId)
        {
            try
            {
                var user = await GetByIdAsync(userId);
                return user?.LoginAttempt ?? 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error getting failed login count for user ID: {userId}", ex);
            }
        }

        public async Task ResetFailedLoginCountAsync(int userId)
        {
            try
            {
                var user = await GetByIdAsync(userId);
                if (user != null)
                {
                    user.LoginAttempt = 0;
                    user.LockoutEnd = null;
                    Update(user);
                    await SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error resetting failed login count for user ID: {userId}", ex);
            }
        }

        public async Task UpdateFailedLoginCount(int userId, int failedCount)
        {
            try
            {
                var user = await GetByIdAsync(userId);
                if (user != null)
                {
                    user.LoginAttempt = failedCount;
                    Update(user);
                    await SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error updating failed login count for user ID: {userId}", ex);
            }
        }

        public async Task LockUserAccountAsync(int userId, DateTime lockoutEnd)
        {
            try
            {
                var user = await GetByIdAsync(userId);
                if (user != null)
                {
                    user.LockoutEnd = lockoutEnd;
                    Update(user);
                    await SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error locking user account for user ID: {userId}", ex);
            }
        }

        public async Task UnlockUserAccountAsync(int userId)
        {
            try
            {
                var user = await GetByIdAsync(userId);
                if (user != null)
                {
                    user.LockoutEnd = null;
                    user.LoginAttempt = 0;
                    Update(user);
                    await SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error unlocking user account for user ID: {userId}", ex);
            }
        }

        // ===== OVERRIDE BASE METHODS FOR SPECIFIC BEHAVIOR =====

        public override async Task AddAsync(UserAccount entity)
        {
            // Ensure username and email are unique before adding
            if (await UsernameExistsAsync(entity.Username))
                throw new ValidationException($"Username '{entity.Username}' already exists");

            if (await EmailExistsAsync(entity.Email))
                throw new ValidationException($"Email '{entity.Email}' already exists");

            await base.AddAsync(entity);
        }

        public override void Update(UserAccount entity)
        {
            // Additional validation can be added here if needed
            base.Update(entity);
        }
    }
}
