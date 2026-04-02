using System;
using KameliaRecycleSystem.Core.Enums;
using KameliaRecycleSystem.Shared.Constants;

namespace KameliaRecycleSystem.Core.Entities
{
    /// <summary>
    /// Entity for resident contributions (iuran) management
    /// Perfectly integrated with Warga, Penerimaan, and approval workflow
    /// Follows established patterns from existing entities
    /// </summary>
    public class IuranWarga : BaseEntity
    {
        // ===== CORE TRANSACTION INFO =====
        public int WargaId { get; set; }                    // Required foreign key to Warga
        public DateTime TanggalIuran { get; set; }          // Payment date
        public decimal JumlahIuran { get; set; }            // Contribution amount
        public string Periode { get; set; }                 // Format: "YYYY-MM" or "YYYY-MM-DD"
        public string JenisIuran { get; set; }              // Rutin, Khusus, Sukarela, dll
        
        // ===== PAYMENT & STATUS INFO =====
        public StatusPersetujuan StatusPembayaran { get; set; } = StatusPersetujuan.Pending;
        public string MetodePembayaran { get; set; }        // Tunai, Transfer, dll
        public string NoBukti { get; set; }                 // Payment receipt number
        public DateTime? TanggalBayar { get; set; }         // Actual payment date
        
        // ===== APPROVAL WORKFLOW (sesuai pola Penerimaan/Pengeluaran) =====
        public int? DisetujuiOleh { get; set; }
        public DateTime? TanggalDisetujui { get; set; }
        public string Catatan { get; set; }                 // Optional notes
        
        // ===== NAVIGATION PROPERTIES =====
        public virtual Warga Warga { get; set; }            // Required relationship
        public virtual UserAccount DisetujuiOlehUser { get; set; }

        // ===== CONSTRUCTORS (sesuai pola entities lain) =====
        public IuranWarga() { }
        
        public IuranWarga(int wargaId, DateTime tanggalIuran, decimal jumlahIuran, string periode, string jenisIuran)
        {
            WargaId = wargaId;
            TanggalIuran = tanggalIuran;
            JumlahIuran = jumlahIuran;
            Periode = periode ?? throw new ArgumentNullException(nameof(periode));
            JenisIuran = jenisIuran ?? throw new ArgumentNullException(nameof(jenisIuran));
            StatusPembayaran = StatusPersetujuan.Pending;
            CreatedDate = DateTime.Now;
            
            ValidateBusinessRules();
        }

        // ===== BUSINESS METHODS (sesuai pola entities lain) =====

        /// <summary>
        /// Mark payment as completed with approval
        /// Follows Penerimaan/Pengeluaran approval pattern
        /// </summary>
        public void SetujuiPembayaran(int disetujuiOlehUserId, string noBukti = null, string metodePembayaran = null)
        {
            if (StatusPembayaran == StatusPersetujuan.Disetujui)
                throw new InvalidOperationException("Pembayaran sudah disetujui sebelumnya");
                
            StatusPembayaran = StatusPersetujuan.Disetujui;
            DisetujuiOleh = disetujuiOlehUserId;
            TanggalDisetujui = DateTime.Now;
            TanggalBayar = TanggalBayar ?? DateTime.Now;
            
            if (!string.IsNullOrEmpty(noBukti))
                NoBukti = noBukti;
                
            if (!string.IsNullOrEmpty(metodePembayaran))
                MetodePembayaran = metodePembayaran;
                
            ModifiedDate = DateTime.Now;
        }

        /// <summary>
        /// Reject payment with reason
        /// Follows Pengeluaran rejection pattern
        /// </summary>
        public void TolakPembayaran(int disetujuiOlehUserId, string alasan)
        {
            if (StatusPembayaran == StatusPersetujuan.Ditolak)
                throw new InvalidOperationException("Pembayaran sudah ditolak sebelumnya");

            StatusPembayaran = StatusPersetujuan.Ditolak;
            DisetujuiOleh = disetujuiOlehUserId;
            TanggalDisetujui = DateTime.Now;
            Catatan = alasan ?? throw new ArgumentNullException(nameof(alasan));
            ModifiedDate = DateTime.Now;
        }

        /// <summary>
        /// Record payment without approval (for immediate payments)
        /// </summary>
        public void Bayar(string metodePembayaran, string noBukti = null)
        {
            if (StatusPembayaran == StatusPersetujuan.Disetujui)
                throw new InvalidOperationException("Pembayaran sudah disetujui");

            MetodePembayaran = metodePembayaran ?? throw new ArgumentNullException(nameof(metodePembayaran));
            NoBukti = noBukti;
            TanggalBayar = DateTime.Now;
            StatusPembayaran = StatusPersetujuan.Disetujui; // Auto-approve immediate payments
            ModifiedDate = DateTime.Now;
        }

        /// <summary>
        /// Check if payment needs approval
        /// Consistent with Penerimaan.ButuhPersetujuan() pattern
        /// </summary>
        public bool ButuhPersetujuan()
        {
            return StatusPembayaran == StatusPersetujuan.Pending;
        }

        /// <summary>
        /// Check if payment is already approved
        /// Consistent with Penerimaan.SudahDisetujui() pattern
        /// </summary>
        public bool SudahDisetujui()
        {
            return StatusPembayaran == StatusPersetujuan.Disetujui;
        }

        /// <summary>
        /// Check if payment is rejected
        /// Consistent with Penerimaan.Ditolak() pattern
        /// </summary>
        public bool Ditolak()
        {
            return StatusPembayaran == StatusPersetujuan.Ditolak;
        }

        /// <summary>
        /// Check if payment is overdue (based on period)
        /// </summary>
        public bool IsTerlambat()
        {
            if (SudahDisetujui() || string.IsNullOrEmpty(Periode))
                return false;

            // Parse period (assuming format "YYYY-MM")
            if (DateTime.TryParse($"{Periode}-01", out DateTime periodeDate))
            {
                var batasAkhir = periodeDate.AddMonths(1); // End of the period month
                return DateTime.Now > batasAkhir && ButuhPersetujuan();
            }
            
            return false;
        }

        /// <summary>
        /// Calculate late fee if payment is overdue
        /// </summary>
        public decimal HitungDenda()
        {
            if (!IsTerlambat() || string.IsNullOrEmpty(Periode))
                return 0;

            // Parse period and calculate months overdue
            if (DateTime.TryParse($"{Periode}-01", out DateTime periodeDate))
            {
                var bulanTerlambat = (DateTime.Now.Year - periodeDate.Year) * 12 + 
                                   DateTime.Now.Month - periodeDate.Month;
                
                if (bulanTerlambat > 0)
                {
                    // 10% late fee per month (adjustable)
                    return JumlahIuran * 0.10m * bulanTerlambat;
                }
            }
            
            return 0;
        }

        /// <summary>
        /// Get total amount including late fee
        /// </summary>
        public decimal GetTotalAmount()
        {
            return JumlahIuran + HitungDenda();
        }

        /// <summary>
        /// Validate business rules before persistence
        /// Consistent with entity validation pattern
        /// </summary>
        public void ValidateBusinessRules()
        {
            if (WargaId <= 0)
                throw new InvalidOperationException("WargaId harus valid");

            if (JumlahIuran < AppConstants.MinimumIuranAmount)
                throw new InvalidOperationException($"Jumlah iuran minimal {AppConstants.MinimumIuranAmount}");

            if (JumlahIuran > AppConstants.MaximumTransactionAmount)
                throw new InvalidOperationException($"Jumlah iuran melebihi batas maksimal");

            if (string.IsNullOrWhiteSpace(Periode))
                throw new InvalidOperationException("Periode iuran harus diisi");

            if (string.IsNullOrWhiteSpace(JenisIuran))
                throw new InvalidOperationException("Jenis iuran harus diisi");

            // Validate period format (YYYY-MM or YYYY-MM-DD)
            if (!System.Text.RegularExpressions.Regex.IsMatch(Periode, @"^\d{4}-(0[1-9]|1[0-2])(-\d{2})?$"))
                throw new InvalidOperationException("Format periode tidak valid. Gunakan YYYY-MM atau YYYY-MM-DD");
        }

        /// <summary>
        /// Update contribution details
        /// </summary>
        public void UpdateDetails(decimal jumlahIuran, string periode, string jenisIuran, string catatan = null)
        {
            if (SudahDisetujui())
                throw new InvalidOperationException("Tidak dapat mengubah iuran yang sudah disetujui");

            JumlahIuran = jumlahIuran;
            Periode = periode ?? throw new ArgumentNullException(nameof(periode));
            JenisIuran = jenisIuran ?? throw new ArgumentNullException(nameof(jenisIuran));
            Catatan = catatan;
            ModifiedDate = DateTime.Now;
            
            ValidateBusinessRules();
        }

        /// <summary>
        /// Get display information for UI
        /// </summary>
        public string GetDisplayInfo()
        {
            var statusText = StatusPembayaran switch
            {
                StatusPersetujuan.Pending => "Menunggu",
                StatusPersetujuan.Disetujui => "Lunas",
                StatusPersetujuan.Ditolak => "Ditolak",
                StatusPersetujuan.Ditunda => "Ditunda",
                _ => "Tidak Diketahui"
            };

            return $"{Periode} - {AppConstants.FormatCurrency(JumlahIuran)} - {statusText}";
        }

        // ===== OVERRIDE METHODS =====
        public override string ToString()
        {
            return $"Iuran: {Periode} - {JenisIuran} - {AppConstants.FormatCurrency(JumlahIuran)}";
        }
    }
}