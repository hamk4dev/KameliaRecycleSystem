using System;

namespace KameliaRecycleSystem.Core.Exceptions
{
    /// <summary>
    /// Base exception for domain-specific business rule violations
    /// Used across all services and workflows in the system
    /// </summary>
    public class DomainException : Exception
    {
        public string ErrorCode { get; }
        public string UserFriendlyMessage { get; }
        public int? HttpStatusCode { get; }

        public DomainException()
        {
        }

        public DomainException(string message) 
            : base(message)
        {
        }

        public DomainException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        public DomainException(string errorCode, string message, string userFriendlyMessage = null, int? httpStatusCode = null)
            : base(message)
        {
            ErrorCode = errorCode;
            UserFriendlyMessage = userFriendlyMessage ?? message;
            HttpStatusCode = httpStatusCode;
        }

        public DomainException(string errorCode, string message, Exception innerException, string userFriendlyMessage = null, int? httpStatusCode = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            UserFriendlyMessage = userFriendlyMessage ?? message;
            HttpStatusCode = httpStatusCode;
        }

        // Common domain exception factory methods
        public static DomainException BusinessRuleViolation(string rule, string details)
        {
            return new DomainException(
                errorCode: "BUSINESS_RULE_VIOLATION",
                message: $"Business rule violation: {rule}. Details: {details}",
                userFriendlyMessage: "Operasi tidak dapat dilakukan karena melanggar aturan bisnis."
            );
        }

        public static DomainException InvalidOperation(string operation, string reason)
        {
            return new DomainException(
                errorCode: "INVALID_OPERATION",
                message: $"Invalid operation: {operation}. Reason: {reason}",
                userFriendlyMessage: "Operasi tidak valid untuk kondisi saat ini."
            );
        }

        public static DomainException ConcurrencyConflict(string entity, string identifier)
        {
            return new DomainException(
                errorCode: "CONCURRENCY_CONFLICT",
                message: $"Concurrency conflict detected for {entity} with identifier: {identifier}",
                userFriendlyMessage: "Data telah diubah oleh pengguna lain. Silakan refresh dan coba lagi."
            );
        }
    }
}