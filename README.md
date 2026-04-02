# Kamelia Recycle System

![Platform](https://img.shields.io/badge/platform-Windows-0A66C2)
![Framework](https://img.shields.io/badge/.NET-8.0-512BD4)
![UI](https://img.shields.io/badge/UI-Windows%20Forms-1F6FEB)
![Status](https://img.shields.io/badge/status-Prototype-F59E0B)

Kamelia Recycle System merupakan aplikasi desktop berbasis Windows yang disiapkan untuk mendukung pengelolaan TPS3R secara lebih tertib, terukur, dan terdokumentasi. Sistem ini diarahkan untuk menangani administrasi warga, iuran, transaksi keuangan, kontrol akses, pelaporan, serta kebutuhan operasional yang memerlukan jejak audit.

Pada tahap saat ini, repositori berfungsi sebagai fondasi pengembangan. Struktur modul utama telah disusun, alur autentikasi awal sudah berjalan, dan dashboard dasar telah tersedia sebagai titik masuk aplikasi.

## Profil Singkat

| Aspek | Deskripsi |
| --- | --- |
| Nama sistem | `Kamelia Recycle System` |
| Jenis aplikasi | Desktop application |
| Platform target | Windows |
| Basis teknologi | .NET 8, Windows Forms, Entity Framework Core |
| Domain utama | TPS3R, warga, iuran, keuangan, pelaporan, audit |
| Tahap pengembangan | Prototype foundation |

## Arah dan Tujuan Pengembangan

Proyek ini dikembangkan untuk menghadirkan sistem administrasi TPS3R yang lebih terpusat dan lebih mudah diaudit dibandingkan proses manual. Dalam pengembangannya, sistem diarahkan untuk mengurangi pencatatan berulang, memperjelas status data operasional, memperkuat pengendalian akses pengguna, dan menyediakan dasar yang layak untuk berkembang menjadi aplikasi operasional harian.

## Status Implementasi

| Area | Status | Keterangan |
| --- | --- | --- |
| Startup aplikasi | Tersedia | Mekanisme startup dan logging error telah tersedia |
| Login | Tersedia | Login dasar telah terhubung ke dashboard |
| Dashboard utama | Tersedia | Berfungsi sebagai landing dashboard awal |
| Modul warga | Draft | Struktur dasar tersedia, implementasi penuh belum selesai |
| Modul iuran | Draft | Entity dan kontrak dasar telah disiapkan |
| Modul keuangan | Draft | Fondasi modul tersedia, integrasi end-to-end belum final |
| Pelaporan | Draft | Generator dan exporter masih berupa pondasi |
| Backup dan restore | Draft | Struktur kerja tersedia, belum final untuk operasional |
| Persistensi data | Sementara | Masih menggunakan `InMemoryDatabase` untuk audit awal |

## Cakupan Arsitektur

Repositori ini memakai pendekatan modular agar pengembangan dapat dilakukan bertahap tanpa mengubah fondasi sistem secara berulang.

| Lapisan / Folder | Fungsi utama |
| --- | --- |
| `Core/` | Menyimpan entity domain, DTO, enum, exception, dan interface |
| `Application/` | Menyimpan service dan validator tingkat aplikasi |
| `Infrastructure/` | Menyimpan repository, EF Core context, dan implementasi security |
| `Presentation/` | Menyimpan Windows Forms, custom control, dan view model |
| `Business/` | Menyimpan service bisnis, calculator, dan validator tambahan |
| `Reporting/` | Menyimpan generator laporan, exporter, printer, dan template |
| `Security/` | Menyimpan scaffold autentikasi, otorisasi, dan enkripsi |
| `Data/` | Menyimpan struktur model dan repository target arsitektur |
| `Backup/` | Menyimpan komponen backup, restore, verifikasi, dan scheduler |
| `Utilities/` | Menyimpan helper, extension, constants, dan logging umum |
| `Configuration/` | Menyimpan representasi konfigurasi aplikasi |
| `DataStorage/` | Menyimpan kebutuhan runtime seperti database, backup, export, temp, dan log |

## Alur Aplikasi Saat Ini

Alur kerja aplikasi pada versi saat ini dapat diringkas sebagai berikut:

```text
Program.cs
   -> Inisialisasi aplikasi
   -> Menyiapkan context dan data audit awal
   -> Menampilkan LoginForm
   -> Memproses autentikasi melalui service keamanan
   -> Mengarahkan pengguna ke MainDashboardForm
```

## Teknologi yang Digunakan

| Teknologi | Peran dalam proyek |
| --- | --- |
| .NET 8 | Runtime dan fondasi aplikasi |
| Windows Forms | Antarmuka desktop |
| Entity Framework Core | Data access dan context model |
| BCrypt | Hash password |
| JWT | Model token untuk session dan autentikasi |

## Menjalankan Proyek

### Dari source code

```powershell
dotnet build
dotnet run
```

### Dari hasil publish Windows

```text
artifacts/publish/win-x64/KameliaRecycleSystem.exe
```

## Akun Audit Sementara

| Field | Nilai |
| --- | --- |
| Username | `admin` |
| Password | `Admin123` |

> Akun ini disediakan untuk verifikasi awal aplikasi dan tidak ditujukan sebagai konfigurasi produksi.

## Roadmap Pengembangan

| Fase | Fokus utama | Hasil yang dituju |
| --- | --- | --- |
| Fase 1 - Stabilisasi Fondasi | Penyempurnaan startup, login, logout, dashboard, serta perapihan dependency flow | Aplikasi stabil untuk audit dasar dan struktur kode lebih konsisten |
| Fase 2 - Persistensi Data | Migrasi dari `InMemoryDatabase` ke SQLite serta penyiapan migrasi database | Data tetap tersimpan setelah aplikasi ditutup |
| Fase 3 - Modul Operasional Inti | Penyelesaian warga, iuran, pemasukan, pengeluaran, dan role dasar | Modul inti siap dipakai secara fungsional |
| Fase 4 - Pelaporan dan Audit | Integrasi laporan dengan data riil, ekspor dokumen, dan activity log | Sistem mampu menghasilkan laporan dan audit trail dasar |
| Fase 5 - Kesiapan Operasional | Penyempurnaan backup, restore, validasi, dan pengujian | Sistem lebih siap dipakai pada operasional harian |

## Prioritas Pengembangan Terdekat

| Urutan | Agenda kerja |
| --- | --- |
| 1 | Finalisasi model data inti |
| 2 | Migrasi ke SQLite |
| 3 | Integrasi repository dengan use case nyata |
| 4 | Penyelesaian form manajemen warga |
| 5 | Penyelesaian modul iuran |
| 6 | Penyelesaian modul keuangan |
| 7 | Penyelesaian pelaporan berbasis data nyata |

## Catatan Teknis

Kondisi teknis saat ini masih menunjukkan bahwa proyek berada pada tahap awal pengembangan. Penyimpanan data masih bersifat sementara karena menggunakan `InMemoryDatabase`, dan sebagian modul masih disusun sebagai scaffold agar struktur besar sistem dapat dibentuk terlebih dahulu. Build proyek saat ini tetap berjalan, namun masih menyisakan warning non-blocking pada konfigurasi EF Core terkait `HasCheckConstraint`, yang sebaiknya dirapikan pada tahap stabilisasi fondasi.

## Pendekatan Pengembangan yang Disarankan

Pengembangan lanjutan paling aman dilakukan dengan pendekatan bertahap: satu modul diselesaikan secara end-to-end, kemudian diperkuat dengan validasi, logging, dan penyimpanan data yang stabil, sebelum berpindah ke modul berikutnya. Pendekatan ini akan membuat progres proyek lebih terukur dan menjaga kualitas implementasi tetap konsisten.

## Penutup

Repositori ini disiapkan sebagai basis pengembangan awal untuk akun GitHub `hamk4dev`. Dengan fondasi yang sudah ada saat ini, proyek siap dilanjutkan menuju fase integrasi data, penyempurnaan modul inti, dan penyiapan sistem yang lebih siap digunakan dalam operasional nyata.
