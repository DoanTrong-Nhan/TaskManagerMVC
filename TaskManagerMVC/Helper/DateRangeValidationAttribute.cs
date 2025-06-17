using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TaskManagerMVC.Helper
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DateRangeValidationAttribute : ValidationAttribute
    {
        private readonly string _startDateProperty;
        private readonly string _dueDateProperty;
        private readonly string _format;

        public DateRangeValidationAttribute(string startDateProperty, string dueDateProperty, string format = "dd/MM/yyyy")
        {
            _startDateProperty = startDateProperty;
            _dueDateProperty = dueDateProperty;
            _format = format;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startProp = validationContext.ObjectType.GetProperty(_startDateProperty);
            var dueProp = validationContext.ObjectType.GetProperty(_dueDateProperty);

            if (startProp == null || dueProp == null)
            {
                return new ValidationResult("Invalid property names.");
            }

            string startStr = startProp.GetValue(validationContext.ObjectInstance) as string;
            string dueStr = dueProp.GetValue(validationContext.ObjectInstance) as string;

            if (string.IsNullOrWhiteSpace(startStr) || string.IsNullOrWhiteSpace(dueStr))
                return ValidationResult.Success;

            if (DateTime.TryParseExact(startStr, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDate) &&
                DateTime.TryParseExact(dueStr, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dueDate))
            {
                if (startDate > dueDate)
                {
                    return new ValidationResult("Start date must be earlier than or equal to due date.");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date format for StartDate or DueDate.");
        }
    }
}
