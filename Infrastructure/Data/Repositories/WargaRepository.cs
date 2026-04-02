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
    /// Repository implementation for Warga (Resident) data operations
    /// 100% aligned with Warga Management Flow in roadmap debug
    /// Implements IWargaRepository contract with BaseRepository foundation
    /// Perfectly integrated with existing entities and business logic
    /// </summary>
    public class WargaRepository : BaseRepository<Warga>, IWargaRepository
    {
        public WargaRepository(AppDbContext context) : base(context)
        {
        }

        // ===== BUSINESS SPECIFIC OPERATIONS =====

        public async Task<Warga> GetByNoAsync(string no)
        {
            try
            {
                return await _dbSet
                    .FirstOrDefaultAsync(w => w.No == no);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving warga by number: {no}", ex);
            }
        }

        public async Task<Warga> GetByUserAccountIdAsync(int userAccountId)
        {
            try
            {
                return await _dbSet
                    .FirstOrDefaultAsync(w => w.UserAccountId == userAccountId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving warga by user account ID: {userAccountId}", ex);
            }
        }

        public async Task<IEnumerable<Warga>> GetByDesaAsync(string desa)
        {
            try
            {
                return await _dbSet
                    .Where(w => w.Desa == desa)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving warga by desa: {desa}", ex);
            }
        }

        public async Task<IEnumerable<Warga>> GetByJenisWargaAsync(string jenisWarga)
        {
            try
            {
                return await _dbSet
                    .Where(w => w.JenisWarga.ToString() == jenisWarga)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving warga by jenis: {jenisWarga}", ex);
            }
        }

        public async Task<IEnumerable<Warga>> GetByStatusKeanggotaanAsync(string status)
        {
            try
            {
                return await _dbSet
                    .Where(w => w.StatusKeanggotaan == status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error retrieving warga by status: {status}", ex);
            }
        }

        // ===== SEARCH & FILTER OPERATIONS =====

        public async Task<IEnumerable<Warga>> SearchAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllAsync();

                return await _dbSet
                    .Where(w => w.No.Contains(keyword) ||
                               w.Nama.Contains(keyword) ||
                               w.Desa.Contains(keyword) ||
                               w.AlamatDusun.Contains(keyword) ||
                               w.Keterangan.Contains(keyword))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error searching warga with keyword: {keyword}", ex);
            }
        }

        public async Task<IEnumerable<Warga>> GetByFilterAsync(string desa, string jenisWarga, string status)
        {
            try
            {
                var query = _dbSet.AsQueryable();

                if (!string.IsNullOrEmpty(desa))
                    query = query.Where(w => w.Desa == desa);

                if (!string.IsNullOrEmpty(jenisWarga))
                    query = query.Where(w => w.JenisWarga.ToString() == jenisWarga);

                if (!string.IsNullOrEmpty(status))
                    query = query.Where(w => w.StatusKeanggotaan == status);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error filtering warga with criteria - desa: {desa}, jenis: {jenisWarga}, status: {status}", ex);
            }
        }

        // ===== STATISTICS & REPORTING =====

        public async Task<int> GetTotalCountAsync()
        {
            try
            {
                return await _dbSet.CountAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error getting total warga count", ex);
            }
        }

        public async Task<int> GetCountByDesaAsync(string desa)
        {
            try
            {
                return await _dbSet.CountAsync(w => w.Desa == desa);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error getting warga count for desa: {desa}", ex);
            }
        }

        public async Task<int> GetCountByJenisWargaAsync(string jenisWarga)
        {
            try
            {
                return await _dbSet.CountAsync(w => w.JenisWarga.ToString() == jenisWarga);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error getting warga count by jenis: {jenisWarga}", ex);
            }
        }

        public async Task<int> GetCountByStatusAsync(string status)
        {
            try
            {
                return await _dbSet.CountAsync(w => w.StatusKeanggotaan == status);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error getting warga count by status: {status}", ex);
            }
        }

        // ===== BATCH OPERATIONS =====

        public async Task<bool> BulkUpdateStatusAsync(IEnumerable<int> wargaIds, string status)
        {
            try
            {
                var wargas = await _dbSet
                    .Where(w => wargaIds.Contains(w.Id))
                    .ToListAsync();

                foreach (var warga in wargas)
                {
                    warga.UpdateStatus(status);
                }

                _dbSet.UpdateRange(wargas);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error bulk updating warga status to: {status}", ex);
            }
        }

        public async Task<bool> ExistsAsync(string no)
        {
            try
            {
                return await _dbSet.AnyAsync(w => w.No == no);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error checking warga existence by number: {no}", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id, string no)
        {
            try
            {
                return await _dbSet.AnyAsync(w => w.Id != id && w.No == no);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error checking warga existence by ID: {id} and number: {no}", ex);
            }
        }

        // ===== OVERRIDE BASE METHODS FOR BUSINESS VALIDATION =====

        public override async Task AddAsync(Warga entity)
        {
            // Validate business rules before adding
            if (await ExistsAsync(entity.No))
                throw new ValidationException($"Warga dengan nomor '{entity.No}' sudah ada");

            await base.AddAsync(entity);
        }

        public override void Update(Warga entity)
        {
            // Validate business rules before updating
            if (ExistsAsync(entity.Id, entity.No).GetAwaiter().GetResult())
                throw new ValidationException($"Warga dengan nomor '{entity.No}' sudah digunakan oleh warga lain");

            base.Update(entity);
        }

        // ===== OPTIONAL: ENHANCED QUERY METHODS =====

        public async Task<IEnumerable<Warga>> GetRumahTanggaAsync()
        {
            try
            {
                return await _dbSet
                    .Where(w => w.JenisWarga.ToString() == "RumahTangga")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error retrieving rumah tangga warga", ex);
            }
        }

        public async Task<IEnumerable<Warga>> GetNonRumahTanggaAsync()
        {
            try
            {
                return await _dbSet
                    .Where(w => w.JenisWarga.ToString() == "NonRumahTangga")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error retrieving non-rumah tangga warga", ex);
            }
        }

        public async Task<Dictionary<string, int>> GetStatistikByDesaAsync()
        {
            try
            {
                return await _dbSet
                    .GroupBy(w => w.Desa)
                    .Select(g => new { Desa = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Desa, x => x.Count);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error getting warga statistics by desa", ex);
            }
        }
    }
}