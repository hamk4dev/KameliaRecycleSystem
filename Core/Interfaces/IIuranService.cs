using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.Entities.DTOs.Requests;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.Interfaces
{
    /// <summary>
    /// Service contract for resident contribution (iuran) management
    /// Perfectly integrated with IuranWarga entity and IuranRequest DTO
    /// Follows established patterns from IUserService interface
    /// Used in: IuranService.cs → IuranManagement.cs → IuranViewModel.cs
    /// </summary>
    public interface IIuranService
    {
        // ===== BASIC CRUD OPERATIONS =====

        /// <summary>
        /// Create new resident contribution
        /// Integrated with IuranRequest DTO and business validation
        /// </summary>
        Task<IuranWarga> CreateIuranAsync(IuranRequest request);

        /// <summary>
        /// Update existing contribution
        /// Handles business rules and approval workflow
        /// </summary>
        Task<IuranWarga> UpdateIuranAsync(int iuranId, IuranRequest request);

        /// <summary>
        /// Soft delete contribution
        /// Consistent with BaseEntity IsActive pattern
        /// </summary>
        Task<bool> DeleteIuranAsync(int iuranId);

        /// <summary>
        /// Get contribution by ID with full details
        /// Includes Warga and ApprovalUser navigation properties
        /// </summary>
        Task<IuranWarga> GetIuranByIdAsync(int iuranId);

        /// <summary>
        /// Get all contributions with pagination support
        /// Consistent with repository pattern
        /// </summary>
        Task<IEnumerable<IuranWarga>> GetAllIuranAsync(int pageNumber = 1, int pageSize = 20);

        // ===== SPECIFIC QUERIES =====

        /// <summary>
        /// Get contributions by resident ID
        /// Used in resident profile and history
        /// </summary>
        Task<IEnumerable<IuranWarga>> GetIuranByWargaIdAsync(int wargaId);

        /// <summary>
        /// Get contributions by funding source type
        /// Uses SumberDana enum for filtering
        /// </summary>
        Task<IEnumerable<IuranWarga>> GetIuranByJenisAsync(SumberDana jenisIuran);

        /// <summary>
        /// Get contributions by approval status
        /// Integrated with StatusPersetujuan workflow
        /// </summary>
        Task<IEnumerable<IuranWarga>> GetIuranByStatusAsync(StatusPersetujuan status);

        /// <summary>
        /// Get contributions by specific period
        /// Period format: "YYYY-MM" consistent with entity
        /// </summary>
        Task<IEnumerable<IuranWarga>> GetIuranByPeriodeAsync(string periode);

        /// <summary>
        /// Search contributions with multiple criteria
        /// Supports resident name, period, and amount range
        /// </summary>
        Task<IEnumerable<IuranWarga>> SearchIuranAsync(string searchTerm, string periode = null, 
                                                      decimal? minAmount = null, decimal? maxAmount = null);

        // ===== APPROVAL WORKFLOW OPERATIONS =====

        /// <summary>
        /// Approve contribution payment
        /// Follows IuranWarga.SetujuiPembayaran() business logic
        /// </summary>
        Task<bool> ApproveIuranAsync(int iuranId, int disetujuiOlehUserId, string noBukti = null, string metodePembayaran = null);

        /// <summary>
        /// Reject contribution with reason
        /// Follows IuranWarga.TolakPembayaran() business logic
        /// </summary>
        Task<bool> RejectIuranAsync(int iuranId, int disetujuiOlehUserId, string alasan);

        /// <summary>
        /// Record immediate payment without approval
        /// Uses IuranWarga.Bayar() business method
        /// </summary>
        Task<bool> RecordPaymentAsync(int iuranId, string metodePembayaran, string noBukti = null);

        // ===== BUSINESS CALCULATIONS =====

        /// <summary>
        /// Calculate total contributions for a resident
        /// Includes both approved and pending payments
        /// </summary>
        Task<decimal> GetTotalIuranByWargaAsync(int wargaId);

        /// <summary>
        /// Calculate total contributions by period
        /// Used in financial reporting and dashboard
        /// </summary>
        Task<decimal> GetTotalIuranByPeriodeAsync(string periode);

        /// <summary>
        /// Calculate total contributions by type and period
        /// For detailed financial analysis
        /// </summary>
        Task<decimal> GetTotalIuranByJenisPeriodeAsync(SumberDana jenisIuran, string periode);

        /// <summary>
        /// Calculate late fees for overdue contributions
        /// Uses IuranWarga.HitungDenda() business logic
        /// </summary>
        Task<decimal> CalculateTotalDendaByPeriodeAsync(string periode);

        // ===== BATCH OPERATIONS =====

        /// <summary>
        /// Create multiple contributions for bulk processing
        /// Useful for monthly routine contributions
        /// </summary>
        Task<IEnumerable<IuranWarga>> CreateBulkIuranAsync(IEnumerable<IuranRequest> requests);

        /// <summary>
        /// Approve multiple contributions in batch
        /// For efficient workflow processing
        /// </summary>
        Task<bool> ApproveBulkIuranAsync(IEnumerable<int> iuranIds, int disetujuiOlehUserId);

        // ===== STATUS & VALIDATION =====

        /// <summary>
        /// Check if resident has paid for specific period
        /// Prevents duplicate contributions
        /// </summary>
        Task<bool> IsIuranPaidForPeriodAsync(int wargaId, string periode);

        /// <summary>
        /// Get contribution status summary for dashboard
        /// Returns counts by status (Pending, Approved, Rejected)
        /// </summary>
        Task<Dictionary<StatusPersetujuan, int>> GetIuranStatusSummaryAsync(string periode = null);

        /// <summary>
        /// Validate contribution business rules before creation
        /// Uses IuranRequest.ValidateBusinessRules() and additional checks
        /// </summary>
        Task<bool> ValidateIuranAsync(IuranRequest request);

        // ===== REPORTING & ANALYTICS =====

        /// <summary>
        /// Get contribution statistics for reporting
        /// Includes totals, averages, and trends
        /// </summary>
        Task<object> GetIuranStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Get overdue contributions for follow-up
        /// Uses IuranWarga.IsTerlambat() business logic
        /// </summary>
        Task<IEnumerable<IuranWarga>> GetOverdueIuranAsync();

        /// <summary>
        /// Get contributions requiring approval
        /// For workflow management and notifications
        /// </summary>
        Task<IEnumerable<IuranWarga>> GetIuranRequiringApprovalAsync();
    }
}