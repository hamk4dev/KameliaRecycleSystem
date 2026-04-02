# Kamelia Recycle System

Kamelia Recycle System adalah aplikasi desktop berbasis Windows Forms untuk membantu pengelolaan operasional TPS3R, data warga, iuran, keuangan, laporan, keamanan akses, dan kebutuhan administrasi harian.

Repositori ini saat ini berfungsi sebagai fondasi proyek: struktur modul sudah disiapkan, alur login dasar sudah berjalan, dashboard awal sudah tersedia, dan pondasi pengembangan berikutnya sudah dipetakan agar implementasi bisa dilanjutkan secara bertahap.

## Tujuan Proyek

- Menyediakan sistem administrasi TPS3R yang lebih rapi, terpusat, dan mudah diaudit.
- Mempermudah pengelolaan data warga, tagihan iuran, pemasukan, pengeluaran, dan laporan operasional.
- Menyediakan dasar keamanan aplikasi melalui autentikasi, otorisasi, logging, dan audit trail.
- Menjadi basis pengembangan aplikasi operasional yang dapat dipakai secara berkelanjutan di lingkungan kerja nyata.

## Kondisi Saat Ini

Status implementasi saat ini:

- Proyek sudah dapat di-`build` dan di-`publish` untuk Windows.
- Login form sudah terhubung ke dashboard utama.
- Struktur folder utama untuk security, data, business, reporting, backup, utilities, dan configuration sudah tersedia.
- Sebagian file masih berupa scaffold dan placeholder yang disiapkan untuk pengembangan lanjutan.
- Penyimpanan data saat ini masih menggunakan `InMemoryDatabase` untuk kebutuhan audit dan pengujian awal.

## Fitur yang Sudah Tersedia

- Login dasar dengan validasi user.
- Hash password menggunakan BCrypt.
- JWT service dan user session model dasar.
- Dashboard utama awal setelah login.
- Startup error logging ke folder log aplikasi.
- Struktur modul pelaporan, backup, dan utilitas sebagai fondasi pengembangan.

## Keterbatasan Saat Ini

- Belum menggunakan database persisten seperti SQLite atau SQL Server produksi.
- Banyak form manajemen masih berupa kerangka UI awal.
- Laporan dan ekspor belum sepenuhnya terhubung ke data nyata.
- Validasi bisnis masih belum lengkap untuk semua modul.
- Belum ada test otomatis.

## Arsitektur Proyek

Repositori ini menggunakan pendekatan modular agar pengembangan lebih mudah dipelihara.

- `Core/`
  Berisi entitas domain, enum, exception, DTO, dan interface utama.
- `Application/`
  Berisi service dan validator tingkat aplikasi.
- `Infrastructure/`
  Berisi implementasi data access, EF Core context, repository, dan security service.
- `Presentation/`
  Berisi Windows Forms, control, dan view model untuk UI desktop.
- `Business/`
  Berisi kalkulator, service bisnis, dan validator lintas modul.
- `Reporting/`
  Berisi generator laporan, exporter, printer, dan template.
- `Security/`
  Berisi scaffold terpisah untuk autentikasi, otorisasi, dan enkripsi.
- `Data/`
  Berisi scaffold model dan repository versi target struktur sistem.
- `Backup/`
  Berisi scaffold backup, restore, verifikasi, dan scheduler.
- `Utilities/`
  Berisi helper, extension, konstanta, dan komponen logging umum.
- `Configuration/`
  Berisi model konfigurasi aplikasi.
- `DataStorage/`
  Berisi lokasi penyimpanan database, backup, export, temp, dan log runtime.

## Teknologi yang Digunakan

- .NET 8
- Windows Forms
- Entity Framework Core
- BCrypt
- JWT (`System.IdentityModel.Tokens.Jwt`)

## Cara Menjalankan Proyek

### Melalui source code

```powershell
dotnet build
dotnet run
```

### Melalui hasil publish Windows

Executable hasil publish berada di:

```text
artifacts/publish/win-x64/KameliaRecycleSystem.exe
```

## Akun Audit Sementara

Untuk audit awal, gunakan akun berikut:

- Username: `admin`
- Password: `Admin123`

## Rencana Pengembangan

### Fase 1 - Stabilisasi Fondasi

Target:

- Menstabilkan alur startup, login, logout, dan dashboard.
- Merapikan kontrak antar layer yang sudah ada.
- Membersihkan duplikasi struktur antara folder fondasi dan folder target.
- Menetapkan standar naming, dependency flow, dan tanggung jawab per modul.

Deliverable:

- Aplikasi bisa dibuka dan dipakai untuk audit dasar.
- Dokumentasi struktur proyek lebih jelas.
- Error startup dan error login bisa dilacak lewat log.

### Fase 2 - Persistensi Data

Target:

- Mengganti `InMemoryDatabase` menjadi SQLite untuk penyimpanan nyata.
- Menyiapkan migrasi database yang lebih stabil.
- Menetapkan seeding data awal yang aman untuk lingkungan development.

Deliverable:

- Data warga, user, dan transaksi tidak hilang saat aplikasi ditutup.
- Struktur database siap dipakai untuk pengembangan modul inti.

### Fase 3 - Modul Inti Operasional

Target:

- Menyelesaikan CRUD data warga.
- Menyelesaikan modul iuran warga dan histori pembayaran.
- Menyelesaikan modul pemasukan dan pengeluaran.
- Menyelesaikan manajemen user dan role dasar.

Deliverable:

- Admin dapat mengelola data warga.
- Petugas dapat mencatat transaksi iuran dan keuangan.
- Dashboard menampilkan ringkasan operasional dasar.

### Fase 4 - Pelaporan dan Audit

Target:

- Menyambungkan generator laporan ke data nyata.
- Menyelesaikan ekspor PDF, Excel, HTML, dan CSV.
- Menambahkan activity log yang dapat dibaca admin.
- Menambahkan ringkasan audit untuk aktivitas user.

Deliverable:

- Laporan keuangan, warga, dan sampah bisa dihasilkan dari sistem.
- Aktivitas pengguna tercatat untuk kebutuhan audit internal.

### Fase 5 - Operasional Produksi

Target:

- Menyelesaikan backup dan restore.
- Menambahkan validasi data yang lebih kuat.
- Menambahkan pengujian untuk service penting.
- Mempersiapkan aplikasi agar layak dipakai harian.

Deliverable:

- Risiko kehilangan data menurun.
- Kualitas aplikasi lebih stabil.
- Deployment lokal lebih aman untuk operasional lapangan.

## Backlog Prioritas

Daftar prioritas implementasi berikutnya:

1. Migrasi ke SQLite.
2. Menyambungkan dashboard ke data nyata.
3. Menyelesaikan `WargaManagementForm`.
4. Menyelesaikan modul iuran warga.
5. Menyelesaikan modul keuangan.
6. Menambahkan activity log yang benar-benar tersimpan.
7. Menambahkan export laporan berbasis data.

## Catatan Pengembangan

Beberapa bagian proyek saat ini memang masih scaffold. Itu disengaja agar struktur besar sistem terbentuk lebih dulu, kemudian implementasi detail bisa dilakukan per modul tanpa mengulang fondasi proyek.

Pendekatan pengembangan yang disarankan:

1. Stabilkan fondasi.
2. Selesaikan satu modul end-to-end.
3. Tambahkan validasi dan logging.
4. Baru lanjut ke laporan dan automasi operasional.

## Saran Langkah Berikutnya

Jika proyek ini akan dilanjutkan, urutan terbaik adalah:

1. Finalisasi model data inti.
2. Pindah ke SQLite.
3. Hubungkan repository ke use case nyata.
4. Rapikan UI form utama.
5. Tambahkan test untuk service login, warga, dan keuangan.

## Build Notes

Build saat ini masih memiliki warning non-blocking pada konfigurasi EF Core terkait `HasCheckConstraint`. Warning ini tidak menghentikan aplikasi berjalan, tetapi sebaiknya dirapikan pada tahap stabilisasi fondasi.

## Kontributor

Repositori ini saat ini disiapkan sebagai basis pengembangan awal untuk akun GitHub `hamk4dev`.
