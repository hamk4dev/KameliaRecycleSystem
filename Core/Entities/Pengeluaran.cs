using System;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.Entities
{
    public class Pengeluaran : BaseEntity
    {
        // BASIC TRANSACTION INFO
        public DateTime Tanggal { get; set; }
        public decimal Jumlah { get; set; }
        public string Deskripsi { get; set; }
        public string Kategori { get; set; }
        public string MetodePembayaran { get; set; }
        
        // APPROVAL WORKFLOW (sesuai roadmap financial flow)
        public StatusPersetujuan Status { get; set; } = StatusPersetujuan.Pending;
        public int? DisetujuiOleh { get; set; }
        public DateTime? TanggalDisetujui { get; set; }
        public string AlasanPenolakan { get; set; }
        
        // NAVIGATION PROPERTIES
        public virtual UserAccount DisetujuiOlehUser { get; set; }
        
        // CONSTRUCTOR
        public Pengeluaran() { }
        
        public Pengeluaran(DateTime tanggal, decimal jumlah, string deskripsi, string kategori)
        {
            Tanggal = tanggal;
            Jumlah = jumlah;
            Deskripsi = deskripsi ?? throw new ArgumentNullException(nameof(deskripsi));
            Kategori = kategori ?? throw new ArgumentNullException(nameof(kategori));
            Status = StatusPersetujuan.Pending;
            CreatedDate = DateTime.Now;
        }
        
        // BUSINESS METHODS
        public void Setujui(int disetujuiOlehUserId)
        {
            Status = StatusPersetujuan.Disetujui;
            DisetujuiOleh = disetujuiOlehUserId;
            TanggalDisetujui = DateTime.Now;
            AlasanPenolakan = null;
        }
        
        public void Tolak(int disetujuiOlehUserId, string alasan)
        {
            Status = StatusPersetujuan.Ditolak;
            DisetujuiOleh = disetujuiOlehUserId;
            TanggalDisetujui = DateTime.Now;
            AlasanPenolakan = alasan ?? throw new ArgumentNullException(nameof(alasan));
        }
        
        public bool ButuhPersetujuan()
        {
            return Status == StatusPersetujuan.Pending;
        }
        
        public bool SudahDisetujui()
        {
            return Status == StatusPersetujuan.Disetujui;
        }
    }
}