using KameliaRecycleSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KameliaRecycleSystem.Infrastructure.Data.Configurations;

public class PengeluaranConfiguration : IEntityTypeConfiguration<Pengeluaran>
{
    public void Configure(EntityTypeBuilder<Pengeluaran> builder)
    {
        builder.ToTable("Pengeluaran");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Deskripsi).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Kategori).IsRequired().HasMaxLength(100);
        builder.Property(x => x.MetodePembayaran).HasMaxLength(100);
        builder.Property(x => x.Jumlah).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.AlasanPenolakan).HasMaxLength(500);

        builder.HasOne(x => x.DisetujuiOlehUser)
            .WithMany()
            .HasForeignKey(x => x.DisetujuiOleh)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
