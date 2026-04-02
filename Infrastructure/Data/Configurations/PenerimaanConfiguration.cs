using KameliaRecycleSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KameliaRecycleSystem.Infrastructure.Data.Configurations;

public class PenerimaanConfiguration : IEntityTypeConfiguration<Penerimaan>
{
    public void Configure(EntityTypeBuilder<Penerimaan> builder)
    {
        builder.ToTable("Penerimaan");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Sumber).IsRequired().HasMaxLength(100);
        builder.Property(x => x.JenisPenerimaan).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Keterangan).HasMaxLength(500);
        builder.Property(x => x.Jumlah).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasOne(x => x.DisetujuiOlehUser)
            .WithMany()
            .HasForeignKey(x => x.DisetujuiOleh)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
