namespace KameliaRecycleSystem.Core.Enums
{
    /// <summary>
    /// Defines approval statuses for financial transactions and workflows
    /// </summary>
    public enum StatusPersetujuan
    {
        /// <summary>
        /// Waiting for approval
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Transaction has been approved
        /// </summary>
        Disetujui = 2,

        /// <summary>
        /// Transaction has been rejected
        /// </summary>
        Ditolak = 3,

        /// <summary>
        /// Approval process is on hold
        /// </summary>
        Ditunda = 4
    }
}