using System;
using System.Collections.Generic;
using System.Linq;

namespace KameliaRecycleSystem.Core.Exceptions
{
    /// <summary>
    /// Exception for input validation errors
    /// Used by all validators in Application/Validators/ and DTO validation
    /// </summary>
    public class ValidationException : DomainException
    {
        public Dictionary<string, string[]> Errors { get; }
        public IEnumerable<ValidationError> ValidationErrors { get; }

        public ValidationException()
            : base("VALIDATION_ERROR", "One or more validation errors occurred", "Terjadi kesalahan validasi data")
        {
            Errors = new Dictionary<string, string[]>();
            ValidationErrors = Enumerable.Empty<ValidationError>();
        }

        public ValidationException(string message)
            : base("VALIDATION_ERROR", message, "Terjadi kesalahan validasi data")
        {
            Errors = new Dictionary<string, string[]>();
            ValidationErrors = Enumerable.Empty<ValidationError>();
        }

        public ValidationException(string message, Exception innerException)
            : base("VALIDATION_ERROR", message, innerException, "Terjadi kesalahan validasi data")
        {
            Errors = new Dictionary<string, string[]>();
            ValidationErrors = Enumerable.Empty<ValidationError>();
        }

        public ValidationException(Dictionary<string, string[]> errors)
            : base("VALIDATION_ERROR", "One or more validation errors occurred", "Terjadi kesalahan validasi data")
        {
            Errors = errors ?? new Dictionary<string, string[]>();
            ValidationErrors = ConvertToValidationErrors(errors);
        }

        public ValidationException(IEnumerable<ValidationError> validationErrors)
            : base("VALIDATION_ERROR", "One or more validation errors occurred", "Terjadi kesalahan validasi data")
        {
            ValidationErrors = validationErrors ?? Enumerable.Empty<ValidationError>();
            Errors = ConvertToDictionary(validationErrors);
        }

        public ValidationException(string propertyName, string errorMessage)
            : base("VALIDATION_ERROR", $"Validation failed for {propertyName}: {errorMessage}", $"Validasi gagal untuk {propertyName}")
        {
            var errors = new Dictionary<string, string[]>
            {
                [propertyName] = new[] { errorMessage }
            };
            
            Errors = errors;
            ValidationErrors = new[] { new ValidationError(propertyName, errorMessage) };
        }

        // Helper methods to convert between different error formats
        private static IEnumerable<ValidationError> ConvertToValidationErrors(Dictionary<string, string[]> errors)
        {
            var validationErrors = new List<ValidationError>();
            
            foreach (var error in errors)
            {
                foreach (var errorMessage in error.Value)
                {
                    validationErrors.Add(new ValidationError(error.Key, errorMessage));
                }
            }
            
            return validationErrors;
        }

        private static Dictionary<string, string[]> ConvertToDictionary(IEnumerable<ValidationError> validationErrors)
        {
            var errors = new Dictionary<string, string[]>();
            
            foreach (var error in validationErrors)
            {
                if (errors.ContainsKey(error.PropertyName))
                {
                    var existingErrors = errors[error.PropertyName].ToList();
                    existingErrors.Add(error.ErrorMessage);
                    errors[error.PropertyName] = existingErrors.ToArray();
                }
                else
                {
                    errors[error.PropertyName] = new[] { error.ErrorMessage };
                }
            }
            
            return errors;
        }

        // Factory methods for common validation scenarios
        public static ValidationException ForProperty(string propertyName, string errorMessage)
        {
            return new ValidationException(propertyName, errorMessage);
        }

        public static ValidationException ForMultipleErrors(Dictionary<string, string[]> errors)
        {
            return new ValidationException(errors);
        }

        public static ValidationException ForMultipleErrors(IEnumerable<ValidationError> validationErrors)
        {
            return new ValidationException(validationErrors);
        }

        public static ValidationException Required(string propertyName)
        {
            return new ValidationException(
                propertyName, 
                $"{propertyName} is required"
            );
        }

        public static ValidationException InvalidLength(string propertyName, int minLength, int maxLength)
        {
            return new ValidationException(
                propertyName,
                $"{propertyName} must be between {minLength} and {maxLength} characters"
            );
        }

        public static ValidationException InvalidFormat(string propertyName, string expectedFormat)
        {
            return new ValidationException(
                propertyName,
                $"{propertyName} must be in format: {expectedFormat}"
            );
        }

        // Override ToString to include all validation errors
        public override string ToString()
        {
            var baseString = base.ToString();
            if (!Errors.Any() && !ValidationErrors.Any())
                return baseString;

            var errorDetails = string.Join(", ", 
                Errors.SelectMany(e => e.Value.Select(v => $"{e.Key}: {v}")));
            
            return $"{baseString} | Validation Errors: {errorDetails}";
        }
    }

    /// <summary>
    /// Represents a single validation error
    /// </summary>
    public class ValidationError
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }

        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        public override string ToString()
        {
            return $"{PropertyName}: {ErrorMessage}";
        }
    }
}