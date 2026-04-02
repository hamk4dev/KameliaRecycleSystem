using System;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.Entities
{
    public class Penerimaan : BaseEntity
    {
        // BASIC TRANSACTION INFO (sesuai roadmap financial transactions)
        public DateTime Tanggal { get; set; }
        public decimal Jumlah { get; set; }
        public string Sumber { get; set; }              // Sumber penerimaan (iuran, penjualan, dll)
        public string JenisPenerimaan { get; set; }     // Jenis penerimaan (rutin, insidentil, dll)
        public string Keterangan { get; set; }
        
        // APPROVAL WORKFLOW (sesuai roadmap financial flow)
        public StatusPersetujuan Status { get; set; } = StatusPersetujuan.Pending;
        public int? DisetujuiOleh { get; set; }
        public DateTime? TanggalDisetujui { get; set; }
        
        // NAVIGATION PROPERTIES (sesuai relationship design)
        public virtual UserAccount DisetujuiOlehUser { get; set; }
        
        // CONSTRUCTOR
        public Penerimaan() { }
        
        public Penerimaan(DateTime tanggal, decimal jumlah, string sumber, string jenisPenerimaan)
        {
            Tanggal = tanggal;
            Jumlah = jumlah;
            Sumber = sumber ?? throw new ArgumentNullException(nameof(sumber));
            JenisPenerimaan = jenisPenerimaan ?? throw new ArgumentNullException(nameof(jenisPenerimaan));
            Status = StatusPersetujuan.Pending;
            CreatedDate = DateTime.Now;
        }
        
        // BUSINESS METHODS (sesuai workflow requirements)
        public void Setujui(int disetujuiOlehUserId)
        {
            Status = StatusPersetujuan.Disetujui;
            DisetujuiOleh = disetujuiOlehUserId;
            TanggalDisetujui = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
        
        public void Tolak(int disetujuiOlehUserId)
        {
            Status = StatusPersetujuan.Ditolak;
            DisetujuiOleh = disetujuiOlehUserId;
            TanggalDisetujui = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
        
        public bool ButuhPersetujuan()
        {
            return Status == StatusPersetujuan.Pending;
        }
        
        public bool SudahDisetujui()
        {
            return Status == StatusPersetujuan.Disetujui;
        }
        
        public bool Ditolak()
        {
            return Status == StatusPersetujuan.Ditolak;
        }
    }
}