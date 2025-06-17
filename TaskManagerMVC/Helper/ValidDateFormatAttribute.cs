using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TaskManagerMVC.Helper
{
    public class ValidDateFormatAttribute : ValidationAttribute
    {
        private readonly string _format;

        public ValidDateFormatAttribute(string format)
        {
            _format = format;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; 
            }

            string input = value.ToString();
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {

                string formattedDate = parsedDate.ToString(_format, CultureInfo.InvariantCulture);

                var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                propertyInfo.SetValue(validationContext.ObjectInstance, formattedDate);
                return ValidationResult.Success;
            }

            if (DateTime.TryParseExact(input, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
