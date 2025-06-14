using System.ComponentModel.DataAnnotations;
using System.Globalization;
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

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Cho phép giá trị null nếu không bắt buộc
            }

            string input = value.ToString();
            // Thử phân tích với định dạng yyyy-MM-dd (từ input type="date")
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Chuyển đổi thành định dạng dd/MM/yyyy để lưu vào DTO
                string formattedDate = parsedDate.ToString(_format, CultureInfo.InvariantCulture);
                // Gán lại giá trị đã chuyển đổi vào DTO
                var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                propertyInfo.SetValue(validationContext.ObjectInstance, formattedDate);
                return ValidationResult.Success;
            }
            // Thử phân tích với định dạng dd/MM/yyyy (nếu người dùng nhập thủ công)
            if (DateTime.TryParseExact(input, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
