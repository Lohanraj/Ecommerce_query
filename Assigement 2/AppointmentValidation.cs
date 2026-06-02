using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public partial class Appointment
    {
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(PatientName)
                && !string.IsNullOrWhiteSpace(DoctorName)
                && !string.IsNullOrWhiteSpace(Department)
                && ConsultationFee > 0;
        }
    }
}
