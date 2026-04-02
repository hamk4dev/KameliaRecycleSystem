# Kamelia Recycle System

Sistem manajemen TPS3R untuk pengelolaan warga, iuran, keamanan akses, laporan, backup, dan operasional daur ulang berbasis Windows Forms dan .NET 8.

## Status Saat Ini

- Aplikasi sudah dapat dibuild dan dipublish untuk Windows.
- Login sudah terhubung ke dashboard utama.
- Struktur modul utama proyek sudah disiapkan agar pengembangan berikutnya lebih terarah.

## Rencana Proyek

### Fase 1: Fondasi Sistem

- Menstabilkan alur login, session, dan dashboard utama.
- Merapikan struktur folder dan kontrak antar layer.
- Menambahkan konfigurasi dasar database dan logging startup.

### Fase 2: Modul Inti Operasional

- Menyelesaikan manajemen data warga.
- Menyelesaikan modul iuran warga dan histori pembayaran.
- Menyelesaikan modul keuangan untuk pemasukan dan pengeluaran.

### Fase 3: Pelaporan dan Audit

- Menyelesaikan generator laporan keuangan, warga, dan sampah.
- Menambahkan export PDF, Excel, HTML, dan CSV.
- Menambahkan audit aktivitas pengguna dan monitoring dasar.

### Fase 4: Produksi dan Pemeliharaan

- Mengganti penyimpanan sementara menjadi database persisten seperti SQLite.
- Menambahkan backup/restore otomatis.
- Menambah validasi bisnis dan pengujian agar aplikasi siap dipakai harian.

## Akun Audit Sementara

- Username: `admin`
- Password: `Admin123`

## Teknologi

- .NET 8
- Windows Forms
- Entity Framework Core
- JWT Authentication
- BCrypt
