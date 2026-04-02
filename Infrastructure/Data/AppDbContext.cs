using Microsoft.EntityFrameworkCore;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Infrastructure.Data.Configurations;
using Microsoft.Extensions.Logging;

namespace KameliaRecycleSystem.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework DbContext for Kamelia Recycle System
    /// Central database configuration and entity management
    /// Used in: All repositories → AppDbContext → Database
    /// </summary>
    public class AppDbContext : DbContext
    {
        // ===== AUTHENTICATION & USER MANAGEMENT =====
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Warga> Wargas { get; set; }

        // ===== FINANCIAL MANAGEMENT =====
        public DbSet<Pengeluaran> Pengeluaran { get; set; }
        public DbSet<Penerimaan> Penerimaan { get; set; }

        // ===== FUTURE ENTITIES (Placeholder for Minggu 3-6) =====
        // public DbSet<IuranWarga> IuranWarga { get; set; }
        // public DbSet<SampahOlahan> SampahOlahan { get; set; }
        // public DbSet<PenjualanSampah> PenjualanSampah { get; set; }
        // public DbSet<ProdukDaurUlang> ProdukDaurUlang { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Base configuration will be provided via Dependency Injection
            // Additional configuration can be added here if needed
            if (!optionsBuilder.IsConfigured)
            {
                // Fallback configuration for development
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=KameliaRecycleSystem;Trusted_Connection=true;MultipleActiveResultSets=true")
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging(); // Only for development
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== APPLY ENTITY CONFIGURATIONS =====
            // Authentication & User Management
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new WargaConfiguration());

            // Financial Management
            modelBuilder.ApplyConfiguration(new PengeluaranConfiguration());
            modelBuilder.ApplyConfiguration(new PenerimaanConfiguration());

            // Future configurations (Minggu 3-6)
            // modelBuilder.ApplyConfiguration(new IuranConfiguration());
            // modelBuilder.ApplyConfiguration(new SampahConfiguration());

            // ===== GLOBAL QUERY FILTERS =====
            // Soft delete filter - only include active records
            modelBuilder.Entity<UserAccount>().HasQueryFilter(u => u.IsActive);
            modelBuilder.Entity<Warga>().HasQueryFilter(w => w.IsActive);
            modelBuilder.Entity<Pengeluaran>().HasQueryFilter(p => p.IsActive);
            modelBuilder.Entity<Penerimaan>().HasQueryFilter(p => p.IsActive);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // ===== AUTOMATIC AUDIT FIELDS UPDATE =====
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "System"; // Will be replaced with actual user from authentication
                    entry.Entity.IsActive = true;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedDate = DateTime.Now;
                    entry.Entity.ModifiedBy = "System"; // Will be replaced with actual user from authentication
                }
            }

            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                // Log database update exceptions
                throw new Core.Exceptions.DatabaseException("Database update failed", ex);
            }
            catch (Exception ex)
            {
                // Log other exceptions
                throw new Core.Exceptions.DatabaseException("Database operation failed", ex);
            }
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }

        // ===== DATABASE HEALTH CHECK METHODS =====
        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }

        public string GetDatabaseProvider()
        {
            return Database.ProviderName ?? "Unknown";
        }

        public async Task EnsureDatabaseCreatedAsync()
        {
            await Database.EnsureCreatedAsync();
        }

        public async Task EnsureDatabaseDeletedAsync()
        {
            await Database.EnsureDeletedAsync();
        }

        // ===== PERFORMANCE OPTIMIZATION METHODS =====
        public void EnableChangeTracking(bool enable = true)
        {
            ChangeTracker.AutoDetectChangesEnabled = enable;
            ChangeTracker.LazyLoadingEnabled = enable;
            ChangeTracker.QueryTrackingBehavior = enable 
                ? QueryTrackingBehavior.TrackAll 
                : QueryTrackingBehavior.NoTracking;
        }

        public void ClearChangeTracker()
        {
            ChangeTracker.Clear();
        }
    }
}
