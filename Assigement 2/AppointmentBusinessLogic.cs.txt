using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public partial class Appointment
    {
        public string GetDetails()
        {
            return $"ID: {AppointmentId}, " +
                   $"Patient: {PatientName}, " +
                   $"Doctor: {DoctorName}, " +
                   $"Department: {Department}, " +
                   $"Date: {AppointmentDate:dd-MM-yyyy}, " +
                   $"Status: {Status}, " +
                   $"Fee: "+$"${ConsultationFee}";
        }
    }
}
