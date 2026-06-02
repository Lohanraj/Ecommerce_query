using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string Department { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; }

        public decimal ConsultationFee { get; set; }
    }

}
