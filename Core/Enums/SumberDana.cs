namespace KameliaRecycleSystem.Core.Enums
{
    /// <summary>
    /// Defines funding sources for financial transactions and contributions
    /// Used in: IuranWarga.cs, Penerimaan.cs, and financial reporting
    /// Perfectly integrated with existing financial workflow and entities
    /// </summary>
    public enum SumberDana
    {
        /// <summary>
        /// Regular monthly contributions from residents
        /// Used in: IuranWarga.JenisIuran and Penerimaan.Sumber
        /// </summary>
        IuranRutin = 1,

        /// <summary>
        /// Special contributions for specific events or projects
        /// Used in: IuranWarga.JenisIuran for non-routine collections
        /// </summary>
        IuranKhusus = 2,

        /// <summary>
        /// Voluntary contributions from residents
        /// Used in: IuranWarga.JenisIuran for optional payments
        /// </summary>
        IuranSukarela = 3,

        /// <summary>
        /// Income from waste sales and recycling activities
        /// Used in: Penerimaan.Sumber for recycling business income
        /// </summary>
        PenjualanSampah = 4,

        /// <summary>
        /// Donations from external parties or community
        /// Used in: Penerimaan.Sumber for external funding
        /// </summary>
        Donasi = 5,

        /// <summary>
        /// Income from recycled product sales
        /// Used in: Penerimaan.Sumber for product-based income
        /// </summary>
        PenjualanProduk = 6,

        /// <summary>
        /// Other unspecified funding sources
        /// Used as fallback for unclassified transactions
        /// </summary>
        Lainnya = 7
    }
}