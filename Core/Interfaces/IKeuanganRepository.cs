using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.Interfaces
{
    /// <summary>
    /// Repository interface for financial data access operations
    /// Used in: KeuanganService.cs → IKeuanganRepository → KeuanganRepository.cs → AppDbContext.cs → Pengeluaran.cs, Penerimaan.cs, IuranWarga.cs
    /// </summary>
    public interface IKeuanganRepository
    {
        // ===== PENGELUARAN OPERATIONS =====
        Task<Pengeluaran> GetPengeluaranByIdAsync(int id);
        Task<IEnumerable<Pengeluaran>> GetAllPengeluaranAsync();
        Task<IEnumerable<Pengeluaran>> GetPengeluaranPagedAsync(int pageNumber, int pageSize);
        Task AddPengeluaranAsync(Pengeluaran pengeluaran);
        void UpdatePengeluaran(Pengeluaran pengeluaran);
        Task DeletePengeluaranAsync(int id);

        // Pengeluaran Filtering and Search
        Task<IEnumerable<Pengeluaran>> GetPengeluaranByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Pengeluaran>> GetPengeluaranByStatusAsync(StatusPersetujuan status);
        Task<IEnumerable<Pengeluaran>> GetPengeluaranByKategoriAsync(string kategori);
        Task<IEnumerable<Pengeluaran>> SearchPengeluaranAsync(string searchTerm);

        // ===== PENERIMAAN OPERATIONS =====
        Task<Penerimaan> GetPenerimaanByIdAsync(int id);
        Task<IEnumerable<Penerimaan>> GetAllPenerimaanAsync();
        Task<IEnumerable<Penerimaan>> GetPenerimaanPagedAsync(int pageNumber, int pageSize);
        Task AddPenerimaanAsync(Penerimaan penerimaan);
        void UpdatePenerimaan(Penerimaan penerimaan);
        Task DeletePenerimaanAsync(int id);

        // Penerimaan Filtering and Search
        Task<IEnumerable<Penerimaan>> GetPenerimaanByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Penerimaan>> GetPenerimaanByStatusAsync(StatusPersetujuan status);
        Task<IEnumerable<Penerimaan>> GetPenerimaanBySumberAsync(string sumber);
        Task<IEnumerable<Penerimaan>> SearchPenerimaanAsync(string searchTerm);

        // ===== IURAN WARGA OPERATIONS =====
        Task<IuranWarga> GetIuranByIdAsync(int id);
        Task<IEnumerable<IuranWarga>> GetAllIuranAsync();
        Task<IEnumerable<IuranWarga>> GetIuranPagedAsync(int pageNumber, int pageSize);
        Task AddIuranAsync(IuranWarga iuran);
        void UpdateIuran(IuranWarga iuran);
        Task DeleteIuranAsync(int id);

        // Iuran Specific Operations
        Task<IEnumerable<IuranWarga>> GetIuranByWargaAsync(int wargaId);
        Task<IEnumerable<IuranWarga>> GetIuranByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<IuranWarga>> GetIuranByStatusAsync(StatusPersetujuan status);
        Task<decimal> GetTotalIuranByWargaAsync(int wargaId);
        Task<decimal> GetTotalIuranByPeriodAsync(DateTime startDate, DateTime endDate);

        // ===== FINANCIAL SUMMARY OPERATIONS =====
        Task<decimal> GetTotalPengeluaranAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalPenerimaanAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetSaldoAsync();
        Task<Dictionary<string, decimal>> GetPengeluaranByKategoriSummaryAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, decimal>> GetPenerimaanBySumberSummaryAsync(DateTime startDate, DateTime endDate);

        // ===== APPROVAL WORKFLOW OPERATIONS =====
        Task<IEnumerable<Pengeluaran>> GetPengeluaranMenungguPersetujuanAsync();
        Task<IEnumerable<Penerimaan>> GetPenerimaanMenungguPersetujuanAsync();
        Task<IEnumerable<IuranWarga>> GetIuranMenungguPersetujuanAsync();
        Task UpdateStatusPersetujuanAsync(int entityId, string entityType, StatusPersetujuan status, int disetujuiOleh);

        // ===== VALIDATION AND EXISTENCE CHECKS =====
        Task<bool> PengeluaranExistsAsync(int id);
        Task<bool> PenerimaanExistsAsync(int id);
        Task<bool> IuranExistsAsync(int id);

        // ===== BULK AND MAINTENANCE OPERATIONS =====
        Task<int> SaveChangesAsync();
        Task<FinancialSummary> GetFinancialSummaryAsync(DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// Financial summary data structure for reporting
    /// </summary>
    public class FinancialSummary
    {
        public decimal TotalPenerimaan { get; set; }
        public decimal TotalPengeluaran { get; set; }
        public decimal Saldo { get; set; }
        public decimal TotalIuran { get; set; }
        public int TotalTransaksi { get; set; }
        public int TransaksiPending { get; set; }
        public int TransaksiDisetujui { get; set; }
        public int TransaksiDitolak { get; set; }
    }
}