using System;
using System.ComponentModel.DataAnnotations;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.DTOs.Requests
{
    /// <summary>
    /// Data transfer object for expense recording and updates
    /// Used in: PengeluaranForm.cs → KeuanganViewModel.cs → IKeuanganService.cs → KeuanganService.cs
    /// </summary>
    public class PengeluaranRequest
    {
        [Required(ErrorMessage = "Tanggal pengeluaran wajib diisi")]
        public DateTime Tanggal { get; set; }

        [Required(ErrorMessage = "Jumlah pengeluaran wajib diisi")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Jumlah pengeluaran harus lebih dari 0")]
        public decimal Jumlah { get; set; }

        [Required(ErrorMessage = "Deskripsi pengeluaran wajib diisi")]
        [StringLength(500, ErrorMessage = "Deskripsi maksimal 500 karakter")]
        public string Deskripsi { get; set; }

        [Required(ErrorMessage = "Kategori pengeluaran wajib diisi")]
        [StringLength(100, ErrorMessage = "Kategori maksimal 100 karakter")]
        public string Kategori { get; set; }

        [Required(ErrorMessage = "Metode pembayaran wajib diisi")]
        [StringLength(50, ErrorMessage = "Metode pembayaran maksimal 50 karakter")]
        public string MetodePembayaran { get; set; }

        // Status persetujuan default ke Pending untuk new records
        public StatusPersetujuan Status { get; set; } = StatusPersetujuan.Pending;

        public PengeluaranRequest()
        {
            Tanggal = DateTime.Today;
        }

        public PengeluaranRequest(DateTime tanggal, decimal jumlah, string deskripsi, string kategori, string metodePembayaran)
        {
            Tanggal = tanggal;
            Jumlah = jumlah;
            Deskripsi = deskripsi;
            Kategori = kategori;
            MetodePembayaran = metodePembayaran;
            Status = StatusPersetujuan.Pending;
        }
    }
}