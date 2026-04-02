namespace KameliaRecycleSystem.Core.Enums
{
    /// <summary>
    /// Defines user roles for authentication and authorization system
    /// 100% aligned with Authentication Flow roadmap debug
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Full system access - can manage all features and users
        /// </summary>
        SuperAdmin = 1,

        /// <summary>
        /// Administrative access - can manage data and approvals
        /// </summary>
        Admin = 2,

        /// <summary>
        /// Operational staff - can input transactions and basic operations
        /// </summary>
        Pegawai = 3,

        /// <summary>
        /// Resident access - limited to personal data and basic features
        /// </summary>
        Warga = 4
    }
}