using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Shared.Constants;

namespace KameliaRecycleSystem.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity configuration for Warga (Resident)
    /// Defines database schema, constraints, and relationships for resident management
    /// </summary>
    public class WargaConfiguration : IEntityTypeConfiguration<Warga>
    {
        public void Configure(EntityTypeBuilder<Warga> builder)
        {
            // ===== TABLE CONFIGURATION =====
            builder.ToTable("Wargas");
            
            // ===== PRIMARY KEY =====
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Id)
                .ValueGeneratedOnAdd();

            // ===== REQUIRED PROPERTIES =====
            builder.Property(w => w.No)
                .IsRequired()
                .HasMaxLength(AppConstants.CodeMaxLength);

            builder.Property(w => w.Nama)
                .IsRequired()
                .HasMaxLength(AppConstants.MaxNameLength);

            builder.Property(w => w.Desa)
                .IsRequired()
                .HasMaxLength(AppConstants.MaxNameLength);

            builder.Property(w => w.AlamatDusun)
                .IsRequired()
                .HasMaxLength(AppConstants.MaxAddressLength);

            builder.Property(w => w.JenisWarga)
                .IsRequired()
                .HasConversion<string>() // Store enum as string for readability
                .HasMaxLength(20);

            builder.Property(w => w.StatusKeanggotaan)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(w => w.TanggalDaftar)
                .IsRequired();

            // ===== OPTIONAL PROPERTIES =====
            builder.Property(w => w.KategoriNonRT)
                .IsRequired(false)
                .HasConversion<string>() // Store enum as string
                .HasMaxLength(20);

            builder.Property(w => w.Keterangan)
                .IsRequired(false)
                .HasMaxLength(AppConstants.MaxNotesLength);

            builder.Property(w => w.UserAccountId)
                .IsRequired(false);

            // ===== DEFAULT VALUES =====
            builder.Property(w => w.IsActive)
                .HasDefaultValue(true);

            builder.Property(w => w.StatusKeanggotaan)
                .HasDefaultValue("Aktif");

            builder.Property(w => w.TanggalDaftar)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(w => w.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            // ===== INDEXES FOR PERFORMANCE =====
            builder.HasIndex(w => w.No)
                .IsUnique()
                .HasDatabaseName("IX_Wargas_No");

            builder.HasIndex(w => w.Nama)
                .HasDatabaseName("IX_Wargas_Nama");

            builder.HasIndex(w => w.Desa)
                .HasDatabaseName("IX_Wargas_Desa");

            builder.HasIndex(w => w.JenisWarga)
                .HasDatabaseName("IX_Wargas_JenisWarga");

            builder.HasIndex(w => w.KategoriNonRT)
                .HasDatabaseName("IX_Wargas_KategoriNonRT");

            builder.HasIndex(w => w.StatusKeanggotaan)
                .HasDatabaseName("IX_Wargas_StatusKeanggotaan");

            builder.HasIndex(w => w.IsActive)
                .HasDatabaseName("IX_Wargas_IsActive");

            builder.HasIndex(w => w.UserAccountId)
                .IsUnique() // One-to-One relationship
                .HasDatabaseName("IX_Wargas_UserAccountId")
                .HasFilter("[UserAccountId] IS NOT NULL"); // Only index non-null values

            // Composite indexes for common query patterns
            builder.HasIndex(w => new { w.Desa, w.JenisWarga })
                .HasDatabaseName("IX_Wargas_Desa_JenisWarga");

            builder.HasIndex(w => new { w.JenisWarga, w.KategoriNonRT })
                .HasDatabaseName("IX_Wargas_JenisWarga_KategoriNonRT")
                .HasFilter("[KategoriNonRT] IS NOT NULL");

            builder.HasIndex(w => new { w.Desa, w.StatusKeanggotaan, w.IsActive })
                .HasDatabaseName("IX_Wargas_Desa_Status_Active");

            // ===== RELATIONSHIPS =====
            // One-to-One optional relationship with UserAccount
            builder.HasOne(w => w.UserAccount)
                .WithOne(u => u.Warga)
                .HasForeignKey<Warga>(w => w.UserAccountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // One-to-Many: Warga can have many Iuran records (future)
            // builder.HasMany<IuranWarga>()
            //     .WithOne(i => i.Warga)
            //     .HasForeignKey(i => i.WargaId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // ===== DATA CONSTRAINTS =====
            builder.Property(w => w.No)
                .HasAnnotation("MinLength", 1);

            builder.Property(w => w.Nama)
                .HasAnnotation("MinLength", AppConstants.MinNameLength);

            // ===== CHECK CONSTRAINTS (SQL Server) =====
            builder.HasCheckConstraint("CK_Wargas_No_MinLength", 
                $"LEN([No]) >= 1");

            builder.HasCheckConstraint("CK_Wargas_Nama_MinLength", 
                $"LEN([Nama]) >= {AppConstants.MinNameLength}");

            builder.HasCheckConstraint("CK_Wargas_JenisWarga_Kategori", 
                @"([JenisWarga] = 'RumahTangga' AND [KategoriNonRT] IS NULL) OR 
                  ([JenisWarga] = 'NonRumahTangga' AND [KategoriNonRT] IS NOT NULL)");

            builder.HasCheckConstraint("CK_Wargas_StatusKeanggotaan_Values", 
                "[StatusKeanggotaan] IN ('Aktif', 'Non-Aktif', 'Ditolak', 'Menunggu')");

            // ===== COMPUTED COLUMNS =====
            // Computed column for display identification
            // builder.Property(w => w.KodeIdentifikasi)
            //     .HasComputedColumnSql(@"CASE 
            //         WHEN [JenisWarga] = 'RumahTangga' THEN 'RT-' + [No]
            //         WHEN [KategoriNonRT] = 'Masjid' THEN 'MSJ-' + [No]
            //         WHEN [KategoriNonRT] = 'Sekolah' THEN 'SCH-' + [No]
            //         WHEN [KategoriNonRT] = 'RumahMakan' THEN 'RMAK-' + [No]
            //         WHEN [KategoriNonRT] = 'Kios' THEN 'KIS-' + [No]
            //         WHEN [KategoriNonRT] = 'Toko' THEN 'TKO-' + [No]
            //         WHEN [KategoriNonRT] = 'Kantor' THEN 'KTR-' + [No]
            //         WHEN [KategoriNonRT] = 'Pasar' THEN 'PSR-' + [No]
            //         ELSE 'NRT-' + [No]
            //     END", stored: false);

        }
    }
}
