using System;
using System.ComponentModel.DataAnnotations;
using KameliaRecycleSystem.Core.Enums;
using KameliaRecycleSystem.Shared.Constants;

namespace KameliaRecycleSystem.Core.Entities.DTOs.Requests
{
    /// <summary>
    /// Data transfer object for resident contribution (iuran) creation and updates
    /// Perfectly integrated with IuranWarga entity and SumberDana enum
    /// Follows established DTO patterns from LoginRequest, WargaRequest, PengeluaranRequest
    /// Used in: IuranManagement.cs → IuranService.cs → IuranWarga entity
    /// </summary>
    public class IuranRequest
    {
        // ===== REQUIRED PROPERTIES =====

        /// <summary>
        /// Resident ID for contribution association
        /// Required for entity relationship mapping
        /// </summary>
        [Required(ErrorMessage = "WargaId wajib diisi")]
        [Range(1, int.MaxValue, ErrorMessage = "WargaId harus valid")]
        public int WargaId { get; set; }

        /// <summary>
        /// Contribution payment date
        /// Uses standard DateTime for consistency with other DTOs
        /// </summary>
        [Required(ErrorMessage = "Tanggal iuran wajib diisi")]
        public DateTime TanggalIuran { get; set; }

        /// <summary>
        /// Contribution amount with business rule validation
        /// Follows financial transaction patterns from PengeluaranRequest
        /// </summary>
        [Required(ErrorMessage = "Jumlah iuran wajib diisi")]
        [Range(typeof(decimal), "0.01", "999999999.99",
               ErrorMessage = "Jumlah iuran harus antara {1} dan {2}")]
        public decimal JumlahIuran { get; set; }

        /// <summary>
        /// Contribution period in "YYYY-MM" format
        /// Consistent with IuranWarga entity validation
        /// </summary>
        [Required(ErrorMessage = "Periode iuran wajib diisi")]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])$", 
                          ErrorMessage = "Format periode harus YYYY-MM (contoh: 2024-01)")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Periode harus 7 karakter")]
        public string Periode { get; set; }

        /// <summary>
        /// Funding source type using SumberDana enum
        /// Perfectly integrated with the new enum
        /// </summary>
        [Required(ErrorMessage = "Jenis iuran wajib diisi")]
        public SumberDana JenisIuran { get; set; }

        // ===== OPTIONAL PROPERTIES =====

        /// <summary>
        /// Payment method (Cash, Transfer, etc.)
        /// Optional with length validation
        /// </summary>
        [StringLength(50, ErrorMessage = "Metode pembayaran maksimal 50 karakter")]
        public string MetodePembayaran { get; set; }

        /// <summary>
        /// Payment receipt number for tracking
        /// Optional with length validation
        /// </summary>
        [StringLength(50, ErrorMessage = "Nomor bukti maksimal 50 karakter")]
        public string NoBukti { get; set; }

        /// <summary>
        /// Additional notes or comments
        /// Optional with standard notes length validation
        /// </summary>
        [StringLength(AppConstants.MaxNotesLength, ErrorMessage = "Catatan maksimal {1} karakter")]
        public string Catatan { get; set; }

        // ===== CONSTRUCTORS =====

        /// <summary>
        /// Default constructor for model binding and serialization
        /// Consistent with other DTO patterns
        /// </summary>
        public IuranRequest() { }

        /// <summary>
        /// Parameterized constructor for easy object creation
        /// Follows LoginRequest and WargaRequest constructor patterns
        /// </summary>
        public IuranRequest(int wargaId, DateTime tanggalIuran, decimal jumlahIuran, 
                           string periode, SumberDana jenisIuran)
        {
            WargaId = wargaId;
            TanggalIuran = tanggalIuran;
            JumlahIuran = jumlahIuran;
            Periode = periode ?? throw new ArgumentNullException(nameof(periode));
            JenisIuran = jenisIuran;
        }

        // ===== BUSINESS METHODS =====

        /// <summary>
        /// Validate business rules beyond data annotations
        /// Consistent with entity validation patterns
        /// </summary>
        public void ValidateBusinessRules()
        {
            // Pastikan tanggal iuran tidak di masa depan yang tidak wajar
            if (TanggalIuran > DateTime.Now.AddMonths(1))
            {
                throw new ValidationException("Tanggal iuran tidak boleh lebih dari 1 bulan ke depan");
            }

            // Validasi periode tidak di masa lalu yang terlalu jauh (max 1 tahun)
            if (DateTime.TryParse($"{Periode}-01", out DateTime periodeDate))
            {
                if (periodeDate < DateTime.Now.AddYears(-1))
                {
                    throw new ValidationException("Periode iuran tidak boleh lebih dari 1 tahun yang lalu");
                }

                if (periodeDate > DateTime.Now.AddMonths(3))
                {
                    throw new ValidationException("Periode iuran tidak boleh lebih dari 3 bulan ke depan");
                }
            }

            // Validasi khusus untuk jenis iuran tertentu
            if (JenisIuran == SumberDana.IuranRutin && JumlahIuran < AppConstants.MinimumIuranAmount)
            {
                throw new ValidationException($"Iuran rutin minimal {AppConstants.MinimumIuranAmount}");
            }
        }

        /// <summary>
        /// Check if this is a routine contribution
        /// Business logic method consistent with entity patterns
        /// </summary>
        public bool IsIuranRutin()
        {
            return JenisIuran == SumberDana.IuranRutin;
        }

        /// <summary>
        /// Check if this is a special contribution
        /// Business logic method for workflow decisions
        /// </summary>
        public bool IsIuranKhusus()
        {
            return JenisIuran == SumberDana.IuranKhusus;
        }

        /// <summary>
        /// Check if this is a voluntary contribution
        /// Business logic method for reporting and permissions
        /// </summary>
        public bool IsIuranSukarela()
        {
            return JenisIuran == SumberDana.IuranSukarela;
        }

        /// <summary>
        /// Get display information for UI purposes
        /// Consistent with entity ToString patterns
        /// </summary>
        public string GetDisplayInfo()
        {
            return $"{Periode} - {JenisIuran} - {AppConstants.FormatCurrency(JumlahIuran)}";
        }

        /// <summary>
        /// Create a copy of this request for editing
        /// Useful for update operations
        /// </summary>
        public IuranRequest Clone()
        {
            return new IuranRequest
            {
                WargaId = this.WargaId,
                TanggalIuran = this.TanggalIuran,
                JumlahIuran = this.JumlahIuran,
                Periode = this.Periode,
                JenisIuran = this.JenisIuran,
                MetodePembayaran = this.MetodePembayaran,
                NoBukti = this.NoBukti,
                Catatan = this.Catatan
            };
        }

        // ===== OVERRIDE METHODS =====

        public override string ToString()
        {
            return $"IuranRequest: WargaId={WargaId}, Periode={Periode}, Jenis={JenisIuran}, Jumlah={JumlahIuran}";
        }
    }
}
