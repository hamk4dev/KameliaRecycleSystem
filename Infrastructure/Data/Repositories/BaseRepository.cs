using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KameliaRecycleSystem.Core.Exceptions;
using KameliaRecycleSystem.Infrastructure.Data;

namespace KameliaRecycleSystem.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Generic repository implementation with common CRUD operations
    /// 100% aligned with IUserRepository and IKeuanganRepository patterns
    /// Zero duplication - follows existing method signatures exactly
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // ===== BASIC CRUD OPERATIONS (100% MATCH EXISTING INTERFACES) =====
        
        public virtual async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving {typeof(T).Name} with ID {id}", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving all {typeof(T).Name} records", ex);
            }
        }

        // MATCHES IUserRepository.AddAsync AND IKeuanganRepository.AddXxxAsync PATTERN
        public virtual async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                // NOTE: No SaveChangesAsync() here - matches existing pattern
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error adding {typeof(T).Name}", ex);
            }
        }

        // MATCHES IUserRepository.Update AND IKeuanganRepository.UpdateXxx PATTERN  
        public virtual void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                // NOTE: No SaveChangesAsync() here - matches existing pattern
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error updating {typeof(T).Name}", ex);
            }
        }

        // MATCHES IUserRepository.DeleteAsync AND IKeuanganRepository.DeleteXxxAsync PATTERN
        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    // NOTE: No SaveChangesAsync() here - matches existing pattern
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error deleting {typeof(T).Name} with ID {id}", ex);
            }
        }

        // ===== QUERY OPERATIONS (COMPLEMENTARY - NO DUPLICATION) =====

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error finding {typeof(T).Name} records", ex);
            }
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error finding first {typeof(T).Name}", ex);
            }
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error checking existence of {typeof(T).Name}", ex);
            }
        }

        public virtual async Task<int> CountAsync()
        {
            try
            {
                return await _dbSet.CountAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error counting {typeof(T).Name} records", ex);
            }
        }

        // ===== SAVE CHANGES (MATCHES EXISTING INTERFACES) =====
        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error saving changes for {typeof(T).Name}", ex);
            }
        }
    }
}