using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class AppointmentCreateEditDto
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }     
        public int OfficeId { get; set; }     
        
        [DataType(DataType.Date)]
        public DateTime StartDateAndTimeOfAppointment { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDateAndTimeOfAppointment { get; set; }

        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}