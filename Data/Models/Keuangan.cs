namespace KameliaRecycleSystem.Data.Models;

public class Keuangan
{
    public int Id { get; set; }
    public string Kode { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public string Keterangan { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
