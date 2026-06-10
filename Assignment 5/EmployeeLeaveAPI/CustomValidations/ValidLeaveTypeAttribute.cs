using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAPI.CustomValidations
{
    public class ValidLeaveTypeAttribute : ValidationAttribute
    {
        private readonly string[] allowedTypes =
        {
            "Sick",
            "Casual",
            "Earned"
        };

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("LeaveType is required");
            }

            if (!allowedTypes.Contains(value.ToString()))
            {
                return new ValidationResult(
                    "LeaveType must be Sick, Casual, or Earned");
            }

            return ValidationResult.Success;
        }
    }
}