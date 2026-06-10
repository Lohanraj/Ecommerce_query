using System.ComponentModel.DataAnnotations;
using EmployeeLeaveAPI.CustomValidations;

namespace EmployeeLeaveAPI.DTOs
{
    public class LeaveRequestCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string EmployeeName { get; set; }

        [Required]
        [EmailAddress]
        public string EmployeeEmail { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage = "Invalid Indian mobile number")]
        public string MobileNumber { get; set; }

        [Required]
        [ValidLeaveType]
        public string LeaveType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Reason { get; set; }
    }
}