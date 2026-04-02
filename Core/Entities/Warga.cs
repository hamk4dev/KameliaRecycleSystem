using System;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.Entities
{
    public class Warga : BaseEntity
    {
        // IDENTITAS DASAR (sesuai requirement)
        public string No { get; set; }                    // Nomor Urut/Identifikasi
        public string Nama { get; set; }                  // Nama Lengkap
        public string Desa { get; set; }                  // Nama Desa
        public string AlamatDusun { get; set; }           // Alamat Detail per Dusun
        
        // JENIS WARGA (sesuai diskusi)
        public JenisWarga JenisWarga { get; set; }
        public KategoriNonRumahTangga? KategoriNonRT { get; set; } // Optional: hanya untuk non-RT
        
        // RELATIONSHIP DENGAN USER ACCOUNT (optional)
        public int? UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        
        // METADATA & STATUS
        public DateTime TanggalDaftar { get; set; } = DateTime.Now;
        public string StatusKeanggotaan { get; set; } = "Aktif";
        public string Keterangan { get; set; }

        // CONSTRUCTOR
        public Warga() { }
        
        public Warga(string no, string nama, string desa, string alamatDusun, JenisWarga jenisWarga)
        {
            No = no ?? throw new ArgumentNullException(nameof(no));
            Nama = nama ?? throw new ArgumentNullException(nameof(nama));
            Desa = desa ?? throw new ArgumentNullException(nameof(desa));
            AlamatDusun = alamatDusun ?? throw new ArgumentNullException(nameof(alamatDusun));
            JenisWarga = jenisWarga;
            TanggalDaftar = DateTime.Now;
            StatusKeanggotaan = "Aktif";
        }

        // BUSINESS METHODS
        public void SetSebagaiNonRumahTangga(KategoriNonRumahTangga kategori)
        {
            if (JenisWarga != JenisWarga.NonRumahTangga)
            {
                throw new InvalidOperationException("Hanya warga non-rumah tangga yang bisa memiliki kategori");
            }
            KategoriNonRT = kategori;
        }

        public bool IsRumahTangga()
        {
            return JenisWarga == JenisWarga.RumahTangga;
        }

        public bool IsNonRumahTangga()
        {
            return JenisWarga == JenisWarga.NonRumahTangga;
        }

        public string GetKodeIdentifikasi()
        {
            if (IsRumahTangga())
            {
                return $"RT-{No}";
            }
            else
            {
                var prefix = KategoriNonRT?.ToString().Substring(0, 3).ToUpper() ?? "NRT";
                return $"{prefix}-{No}";
            }
        }

        public void UpdateStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status tidak boleh kosong");
                
            StatusKeanggotaan = status;
            ModifiedDate = DateTime.Now;
        }
    }
}