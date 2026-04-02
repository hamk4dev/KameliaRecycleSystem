namespace KameliaRecycleSystem.Shared.Constants
{
    /// <summary>
    /// General application constants for business rules, configurations, and system settings
    /// Used across all layers of the application
    /// </summary>
    public static class AppConstants
    {
        // ===== APPLICATION META DATA =====
        public const string ApplicationName = "Kamelia Recycle System";
        public const string ApplicationVersion = "1.0.0";
        public const string ApplicationDescription = "Sistem Manajemen Daur Ulang dan Keuangan Komunitas";
        public const string CompanyName = "Kamelia Community";
        public const string CopyrightYear = "2024";

        // ===== DATABASE CONFIGURATION =====
        public const string DefaultConnectionStringName = "DefaultConnection";
        public const int DefaultCommandTimeout = 30; // seconds
        public const int BulkOperationTimeout = 300; // seconds
        public const int MaxRetryCount = 3;
        public const int MaxBatchSize = 1000;

        // ===== FILE AND STORAGE CONFIGURATION =====
        public const int MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB
        public const string AllowedImageExtensions = ".jpg,.jpeg,.png,.gif,.bmp";
        public const string AllowedDocumentExtensions = ".pdf,.doc,.docx,.xls,.xlsx,.csv";
        public const string BackupFileExtension = ".bak";
        public const string ExportFileExtension = ".export";
        public const string LogFileExtension = ".log";

        // ===== BUSINESS RULES AND LIMITS =====
        public const decimal MinimumTransactionAmount = 0.01m;
        public const decimal MaximumTransactionAmount = 999_999_999.99m;
        public const int MaxDescriptionLength = 500;
        public const int MaxNotesLength = 1000;
        public const int MaxSearchResults = 100;
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 100;

        // ===== FINANCIAL CONSTANTS =====
        public const string DefaultCurrency = "IDR";
        public const string CurrencyFormat = "N2";
        public const int DecimalPlaces = 2;
        public const decimal MinimumIuranAmount = 1000; // Minimum contribution amount
        public const decimal DefaultIuranAmount = 5000; // Default monthly contribution

        // ===== DATE AND TIME CONSTANTS =====
        public const string DateFormat = "dd/MM/yyyy";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string TimeFormat = "HH:mm";
        public const string TimestampFormat = "yyyyMMddHHmmss";
        public const int DefaultReportRangeDays = 30;
        public const int FinancialYearStartMonth = 1; // January
        public const int FinancialYearEndMonth = 12; // December

        // ===== VALIDATION CONSTANTS =====
        public const int MinNameLength = 2;
        public const int MaxNameLength = 100;
        public const int MinAddressLength = 5;
        public const int MaxAddressLength = 200;
        public const int PhoneNumberMinLength = 10;
        public const int PhoneNumberMaxLength = 15;
        public const int EmailMaxLength = 100;
        public const int CodeMaxLength = 20;

        // ===== SYSTEM BEHAVIOR CONSTANTS =====
        public const int CacheDurationMinutes = 30;
        public const int AutoSaveIntervalSeconds = 30;
        public const int DefaultTimeoutSeconds = 60;
        public const int BackgroundJobIntervalMinutes = 5;
        public const int CleanupJobIntervalHours = 24;

        // ===== USER INTERFACE CONSTANTS =====
        public const int AutoCompleteDelayMs = 300;
        public const int ToolTipDelayMs = 500;
        public const int LoadingSpinnerDelayMs = 200;
        public const int AnimationDurationMs = 300;
        public const int MessageDisplayDurationMs = 5000;

        // ===== REPORTING CONSTANTS =====
        public const string DefaultReportTitle = "Laporan Kamelia Recycle System";
        public const string ReportHeaderFormat = "Laporan {0} - {1}";
        public const int MaxReportRecords = 10000;
        public const string PdfContentType = "application/pdf";
        public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        // ===== NOTIFICATION CONSTANTS =====
        public const int NotificationExpiryDays = 7;
        public const int MaxNotificationLength = 200;
        public const int NotificationBatchSize = 50;

        // ===== EXPORT AND IMPORT CONSTANTS =====
        public const string CsvDelimiter = ",";
        public const string ExportFileNameFormat = "Export_{0}_{1}";
        public const string BackupFileNameFormat = "Backup_{0}_{1}";
        public const int ExportBatchSize = 500;

        // ===== ERROR HANDLING CONSTANTS =====
        public const int MaxErrorLogLength = 4000;
        public const int MaxStackTraceLength = 2000;
        public const int ErrorReportLimit = 1000;

        // ===== PERFORMANCE CONSTANTS =====
        public const int BulkInsertBatchSize = 100;
        public const int QueryTimeoutWarningSeconds = 10;
        public const int MemoryWarningThresholdMB = 500;

        // ===== DEFAULT VALUES =====
        public const string DefaultLanguage = "id-ID";
        public const string DefaultTimeZone = "Asia/Jakarta";
        public const string DefaultCountry = "Indonesia";
        public const string DefaultProvince = "Jawa Barat";
        public const string DefaultCity = "Bandung";

        // ===== BUSINESS SPECIFIC CONSTANTS =====
        public const string DefaultDesa = "Sukamaju";
        public const string DefaultDusun = "Krajan";
        public const string DefaultWargaStatus = "Aktif";
        public const string DefaultIuranDescription = "Iuran Bulanan";
        public const string DefaultPengeluaranCategory = "Operasional";
        public const string DefaultPenerimaanSumber = "Iuran Warga";

        // ===== PATHS AND DIRECTORIES =====
        public const string BackupDirectory = "Backups";
        public const string ExportDirectory = "Exports";
        public const string LogsDirectory = "Logs";
        public const string TemplatesDirectory = "Templates";
        public const string ReportsDirectory = "Reports";
        public const string ImagesDirectory = "Images";

        // ===== FORMATTING METHODS =====
        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString(CurrencyFormat);
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString(DateFormat);
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }

        public static string GenerateExportFileName(string reportType)
        {
            return string.Format(ExportFileNameFormat, reportType, DateTime.Now.ToString(TimestampFormat));
        }

        public static string GenerateBackupFileName()
        {
            return string.Format(BackupFileNameFormat, ApplicationName, DateTime.Now.ToString(TimestampFormat));
        }

        public static string TruncateString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength - 3) + "...";
        }

        public static bool IsValidAmount(decimal amount)
        {
            return amount >= MinimumTransactionAmount && amount <= MaximumTransactionAmount;
        }

        public static bool IsValidDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description) && description.Length <= MaxDescriptionLength;
        }

        // ===== BUSINESS RULE METHODS =====
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var cleanNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return cleanNumber.Length >= PhoneNumberMinLength && cleanNumber.Length <= PhoneNumberMaxLength;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length > EmailMaxLength)
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static decimal CalculateIuranTotal(int monthCount, decimal monthlyAmount)
        {
            return monthCount * monthlyAmount;
        }

        public static DateTime GetFinancialYearStart(DateTime date)
        {
            return new DateTime(date.Year, FinancialYearStartMonth, 1);
        }

        public static DateTime GetFinancialYearEnd(DateTime date)
        {
            return new DateTime(date.Year, FinancialYearEndMonth, DateTime.DaysInMonth(date.Year, FinancialYearEndMonth));
        }

        // ===== VALIDATION METHODS =====
        public static void ValidatePageSize(ref int pageSize)
        {
            if (pageSize <= 0)
                pageSize = DefaultPageSize;
            else if (pageSize > MaxPageSize)
                pageSize = MaxPageSize;
        }

        public static void ValidateSearchTerm(ref string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                if (searchTerm.Length > MaxNameLength)
                    searchTerm = searchTerm.Substring(0, MaxNameLength);
            }
        }
    }
}