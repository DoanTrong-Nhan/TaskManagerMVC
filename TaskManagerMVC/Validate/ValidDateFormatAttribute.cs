using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TaskManagerAPI.Validate
{
    public class ValidDateFormatAttribute : ValidationAttribute
    {
        private readonly string _format;

        public ValidDateFormatAttribute(string format)
        {
            _format = format;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var strValue = value as string;

            if (string.IsNullOrWhiteSpace(strValue))
                return ValidationResult.Success;

            if (DateTime.TryParseExact(strValue, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? $"Định dạng ngày không hợp lệ. Định dạng mong muốn: {_format}");
        }
    }
}
