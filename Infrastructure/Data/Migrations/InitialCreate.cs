using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KameliaRecycleSystem.Infrastructure.Data.Migrations
{
    /// <summary>
    /// Initial database creation migration for Kamelia Recycle System
    /// 100% aligned with Roadmap Development Week 2: Database Setup
    /// Perfectly integrated with existing entities and configurations
    /// </summary>
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ===== CREATE USERACCOUNTS TABLE (AUTHENTICATION FLOW) =====
            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LoginAttempt = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WargaId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.CheckConstraint("CK_UserAccounts_Email_Format", "[Email] LIKE '%_@_%._%'");
                    table.CheckConstraint("CK_UserAccounts_LoginAttempt_Range", "[LoginAttempt] >= 0 AND [LoginAttempt] <= 5");
                    table.CheckConstraint("CK_UserAccounts_Username_MinLength", "LEN([Username]) >= 3");
                });

            // ===== CREATE WARGA TABLE (RESIDENT MANAGEMENT FLOW) =====
            migrationBuilder.CreateTable(
                name: "Wargas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    No = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Desa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlamatDusun = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JenisWarga = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KategoriNonRT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UserAccountId = table.Column<int>(type: "int", nullable: true),
                    TanggalDaftar = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    StatusKeanggotaan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Aktif"),
                    Keterangan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wargas", x => x.Id);
                    table.CheckConstraint("CK_Wargas_JenisWarga_Kategori", "([JenisWarga] = 'RumahTangga' AND [KategoriNonRT] IS NULL) OR ([JenisWarga] = 'NonRumahTangga' AND [KategoriNonRT] IS NOT NULL)");
                    table.CheckConstraint("CK_Wargas_Nama_MinLength", "LEN([Nama]) >= 2");
                    table.CheckConstraint("CK_Wargas_No_MinLength", "LEN([No]) >= 1");
                    table.CheckConstraint("CK_Wargas_StatusKeanggotaan_Values", "[StatusKeanggotaan] IN ('Aktif', 'Non-Aktif', 'Ditolak', 'Menunggu')");
                });

            // ===== CREATE PENGELUARAN TABLE (FINANCIAL FLOW) =====
            migrationBuilder.CreateTable(
                name: "Pengeluaran",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deskripsi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Kategori = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MetodePembayaran = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisetujuiOleh = table.Column<int>(type: "int", nullable: true),
                    TanggalDisetujui = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AlasanPenolakan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pengeluaran", x => x.Id);
                });

            // ===== CREATE PENERIMAAN TABLE (FINANCIAL FLOW) =====
            migrationBuilder.CreateTable(
                name: "Penerimaan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Sumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JenisPenerimaan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisetujuiOleh = table.Column<int>(type: "int", nullable: true),
                    TanggalDisetujui = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penerimaan", x => x.Id);
                });

            // ===== CREATE INDEXES FOR PERFORMANCE (ROADMAP DEBUG OPTIMIZED) =====

            // UserAccounts Indexes (Authentication Flow Optimization)
            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_IsActive",
                table: "UserAccounts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Role",
                table: "UserAccounts",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Role_IsActive",
                table: "UserAccounts",
                columns: new[] { "Role", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Username",
                table: "UserAccounts",
                column: "Username",
                unique: true);

            // Wargas Indexes (Warga Management Flow Optimization)
            migrationBuilder.CreateIndex(
                name: "IX_Wargas_Desa",
                table: "Wargas",
                column: "Desa");

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_Desa_JenisWarga",
                table: "Wargas",
                columns: new[] { "Desa", "JenisWarga" });

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_Desa_Status_Active",
                table: "Wargas",
                columns: new[] { "Desa", "StatusKeanggotaan", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_IsActive",
                table: "Wargas",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_JenisWarga",
                table: "Wargas",
                column: "JenisWarga");

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_JenisWarga_KategoriNonRT",
                table: "Wargas",
                columns: new[] { "JenisWarga", "KategoriNonRT" },
                filter: "[KategoriNonRT] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_KategoriNonRT",
                table: "Wargas",
                column: "KategoriNonRT");

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_Nama",
                table: "Wargas",
                column: "Nama");

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_No",
                table: "Wargas",
                column: "No",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_StatusKeanggotaan",
                table: "Wargas",
                column: "StatusKeanggotaan");

            migrationBuilder.CreateIndex(
                name: "IX_Wargas_UserAccountId",
                table: "Wargas",
                column: "UserAccountId",
                unique: true,
                filter: "[UserAccountId] IS NOT NULL");

            // Pengeluaran Indexes (Financial Flow Optimization)
            migrationBuilder.CreateIndex(
                name: "IX_Pengeluaran_DisetujuiOleh",
                table: "Pengeluaran",
                column: "DisetujuiOleh");

            migrationBuilder.CreateIndex(
                name: "IX_Pengeluaran_Status",
                table: "Pengeluaran",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Pengeluaran_Tanggal",
                table: "Pengeluaran",
                column: "Tanggal");

            // Penerimaan Indexes (Financial Flow Optimization)
            migrationBuilder.CreateIndex(
                name: "IX_Penerimaan_DisetujuiOleh",
                table: "Penerimaan",
                column: "DisetujuiOleh");

            migrationBuilder.CreateIndex(
                name: "IX_Penerimaan_Status",
                table: "Penerimaan",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Penerimaan_Tanggal",
                table: "Penerimaan",
                column: "Tanggal");

            // ===== CREATE FOREIGN KEY RELATIONSHIPS (ROADMAP DEBUG FLOWS) =====

            // Authentication Flow: UserAccount ↔ Warga relationship
            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Wargas_WargaId",
                table: "UserAccounts",
                column: "WargaId",
                principalTable: "Wargas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // Financial Flow: Approval relationships
            migrationBuilder.AddForeignKey(
                name: "FK_Pengeluaran_UserAccounts_DisetujuiOleh",
                table: "Pengeluaran",
                column: "DisetujuiOleh",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Penerimaan_UserAccounts_DisetujuiOleh",
                table: "Penerimaan",
                column: "DisetujuiOleh",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ===== DROP IN REVERSE ORDER (MAINTAINING DEPENDENCIES) =====
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Wargas_WargaId",
                table: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Penerimaan");

            migrationBuilder.DropTable(
                name: "Pengeluaran");

            migrationBuilder.DropTable(
                name: "Wargas");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}