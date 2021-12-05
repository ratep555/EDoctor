using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string Patient { get; set; } 
        public int PatientId { get; set; } 
        public int DoctorId { get; set; } 
        public int OfficeId { get; set; } 
        public int AppointmentId { get; set; }
        public string Doctor { get; set; } 
        public string OfficeAddress { get; set; }     
        public string Hospital { get; set; }     
        public string City { get; set; } 
        public string Country { get; set; } 
            

        [DataType(DataType.Date)]
        public DateTime StartDateAndTimeOfAppointment { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDateAndTimeOfAppointment { get; set; }

        public bool? Status { get; set; }
        public string Remarks { get; set; }
    }
}