using System;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.DTOs.Responses
{
    /// <summary>
    /// Data transfer object for resident data display in UI
    /// Used in: WargaManagement.cs → WargaViewModel.cs → IWargaService.cs → WargaService.cs
    /// </summary>
    public class WargaResponse
    {
        public int Id { get; set; }
        
        // Basic resident information
        public string No { get; set; }
        public string Nama { get; set; }
        public string Desa { get; set; }
        public string AlamatDusun { get; set; }
        
        // Resident type information
        public JenisWarga JenisWarga { get; set; }
        public string JenisWargaDisplay { get; set; }
        public KategoriNonRumahTangga? KategoriNonRT { get; set; }
        public string KategoriNonRTDisplay { get; set; }
        
        // Status and metadata
        public string StatusKeanggotaan { get; set; }
        public string Keterangan { get; set; }
        public DateTime TanggalDaftar { get; set; }
        public string TanggalDaftarDisplay { get; set; }
        
        // User account relationship (if any)
        public int? UserAccountId { get; set; }
        public string Username { get; set; }
        
        // Audit trail
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        
        // Computed properties for UI convenience
        public string KodeIdentifikasi { get; set; }
        public bool IsRumahTangga { get; set; }
        public bool IsNonRumahTangga { get; set; }
        public bool MemilikiAkun { get; set; }

        public WargaResponse()
        {
        }

        public WargaResponse(int id, string no, string nama, string desa, string alamatDusun, 
                           JenisWarga jenisWarga, string statusKeanggotaan, DateTime tanggalDaftar)
        {
            Id = id;
            No = no;
            Nama = nama;
            Desa = desa;
            AlamatDusun = alamatDusun;
            JenisWarga = jenisWarga;
            StatusKeanggotaan = statusKeanggotaan;
            TanggalDaftar = tanggalDaftar;
            
            // Set computed properties
            SetComputedProperties();
        }

        private void SetComputedProperties()
        {
            IsRumahTangga = JenisWarga == JenisWarga.RumahTangga;
            IsNonRumahTangga = JenisWarga == JenisWarga.NonRumahTangga;
            MemilikiAkun = UserAccountId.HasValue;
            
            // Generate display texts
            JenisWargaDisplay = JenisWarga.ToString();
            KategoriNonRTDisplay = KategoriNonRT?.ToString() ?? "-";
            TanggalDaftarDisplay = TanggalDaftar.ToString("dd/MM/yyyy");
            
            // Generate identification code
            KodeIdentifikasi = GenerateKodeIdentifikasi();
        }

        private string GenerateKodeIdentifikasi()
        {
            if (IsRumahTangga)
            {
                return $"RT-{No}";
            }
            else
            {
                var prefix = KategoriNonRT?.ToString().Substring(0, 3).ToUpper() ?? "NRT";
                return $"{prefix}-{No}";
            }
        }
    }
}