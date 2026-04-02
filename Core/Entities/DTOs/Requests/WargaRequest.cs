using System.ComponentModel.DataAnnotations;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.DTOs.Requests
{
    /// <summary>
    /// Data transfer object for resident creation and updates
    /// Used in: WargaManagement.cs → WargaViewModel.cs → IWargaService.cs → WargaService.cs
    /// </summary>
    public class WargaRequest
    {
        [Required(ErrorMessage = "Nomor warga wajib diisi")]
        [StringLength(20, ErrorMessage = "Nomor warga maksimal 20 karakter")]
        public string No { get; set; }

        [Required(ErrorMessage = "Nama warga wajib diisi")]
        [StringLength(100, ErrorMessage = "Nama warga maksimal 100 karakter")]
        public string Nama { get; set; }

        [Required(ErrorMessage = "Desa wajib diisi")]
        [StringLength(50, ErrorMessage = "Nama desa maksimal 50 karakter")]
        public string Desa { get; set; }

        [Required(ErrorMessage = "Alamat dusun wajib diisi")]
        [StringLength(200, ErrorMessage = "Alamat dusun maksimal 200 karakter")]
        public string AlamatDusun { get; set; }

        [Required(ErrorMessage = "Jenis warga wajib dipilih")]
        public JenisWarga JenisWarga { get; set; }

        public KategoriNonRumahTangga? KategoriNonRT { get; set; }

        [StringLength(500, ErrorMessage = "Keterangan maksimal 500 karakter")]
        public string Keterangan { get; set; }

        public WargaRequest()
        {
            // Default constructor
        }

        public WargaRequest(string no, string nama, string desa, string alamatDusun, JenisWarga jenisWarga)
        {
            No = no;
            Nama = nama;
            Desa = desa;
            AlamatDusun = alamatDusun;
            JenisWarga = jenisWarga;
        }
    }
}