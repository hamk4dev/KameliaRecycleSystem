using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.DTOs.Responses;

namespace KameliaRecycleSystem.Core.Interfaces
{
    /// <summary>
    /// Repository interface for Warga (Resident) data operations
    /// Aligned with Warga Management Flow in roadmap debug
    /// </summary>
    public interface IWargaRepository
    {
        // ===== BASIC CRUD OPERATIONS =====
        Task<Warga> GetByIdAsync(int id);
        Task<IEnumerable<Warga>> GetAllAsync();
        Task AddAsync(Warga warga);
        void Update(Warga warga);
        Task DeleteAsync(int id);
        
        // ===== BUSINESS SPECIFIC OPERATIONS =====
        Task<Warga> GetByNoAsync(string no);
        Task<Warga> GetByUserAccountIdAsync(int userAccountId);
        Task<IEnumerable<Warga>> GetByDesaAsync(string desa);
        Task<IEnumerable<Warga>> GetByJenisWargaAsync(string jenisWarga);
        Task<IEnumerable<Warga>> GetByStatusKeanggotaanAsync(string status);
        
        // ===== SEARCH & FILTER OPERATIONS =====
        Task<IEnumerable<Warga>> SearchAsync(string keyword);
        Task<IEnumerable<Warga>> GetByFilterAsync(string desa, string jenisWarga, string status);
        
        // ===== STATISTICS & REPORTING =====
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByDesaAsync(string desa);
        Task<int> GetCountByJenisWargaAsync(string jenisWarga);
        Task<int> GetCountByStatusAsync(string status);
        
        // ===== BATCH OPERATIONS =====
        Task<bool> BulkUpdateStatusAsync(IEnumerable<int> wargaIds, string status);
        Task<bool> ExistsAsync(string no);
        Task<bool> ExistsAsync(int id, string no);
    }
}
