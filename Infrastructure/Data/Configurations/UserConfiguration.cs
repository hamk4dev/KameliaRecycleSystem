using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Shared.Constants;

namespace KameliaRecycleSystem.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity configuration for UserAccount
    /// Defines database schema, constraints, and relationships for user authentication
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            // ===== TABLE CONFIGURATION =====
            builder.ToTable("UserAccounts");
            
            // ===== PRIMARY KEY =====
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            // ===== REQUIRED PROPERTIES =====
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(SecurityConstants.UsernameMaxLength);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255); // BCrypt hash length

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(SecurityConstants.EmailMaxLength);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>() // Store enum as string for readability
                .HasMaxLength(20);

            // ===== OPTIONAL PROPERTIES =====
            builder.Property(u => u.LastLogin)
                .IsRequired(false);

            builder.Property(u => u.LockoutEnd)
                .IsRequired(false);

            // ===== DEFAULT VALUES =====
            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.LoginAttempt)
                .HasDefaultValue(0);

            builder.Property(u => u.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            // ===== INDEXES FOR PERFORMANCE =====
            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("IX_UserAccounts_Username");

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_UserAccounts_Email");

            builder.HasIndex(u => u.Role)
                .HasDatabaseName("IX_UserAccounts_Role");

            builder.HasIndex(u => u.IsActive)
                .HasDatabaseName("IX_UserAccounts_IsActive");

            builder.HasIndex(u => new { u.Role, u.IsActive })
                .HasDatabaseName("IX_UserAccounts_Role_IsActive");

            // ===== RELATIONSHIPS =====
            // One-to-One optional relationship with Warga
            builder.HasOne(u => u.Warga)
                .WithOne(w => w.UserAccount)
                .HasForeignKey<Warga>(w => w.UserAccountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); // If user deleted, set Warga.UserAccountId to null

            // ===== DATA CONSTRAINTS =====
            builder.Property(u => u.Username)
                .HasAnnotation("MinLength", SecurityConstants.UsernameMinLength);

            builder.Property(u => u.PasswordHash)
                .HasAnnotation("MinLength", SecurityConstants.PasswordMinLength);

            // ===== CHECK CONSTRAINTS (SQL Server) =====
            builder.HasCheckConstraint("CK_UserAccounts_Username_MinLength", 
                $"LEN([Username]) >= {SecurityConstants.UsernameMinLength}");

            builder.HasCheckConstraint("CK_UserAccounts_Email_Format", 
                "[Email] LIKE '%_@_%._%'"); // Basic email format validation

            builder.HasCheckConstraint("CK_UserAccounts_LoginAttempt_Range", 
                $"[LoginAttempt] >= 0 AND [LoginAttempt] <= {SecurityConstants.MaxLoginAttempts}");

            // ===== COMPUTED COLUMNS (if needed) =====
            // Example: Computed column for display name
            // builder.Property(u => u.DisplayName)
            //     .HasComputedColumnSql("[Username] + ' (' + [Role] + ')'", stored: true);

        }
    }
}
