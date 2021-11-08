using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Appointment : BaseEntity
    {
        public int? PatientId { get; set; }     
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } 

        public int Office1d { get; set; }     
        [ForeignKey("OfficeId")]
        public Office Office { get; set; }     


        [DataType(DataType.Date)]
        public DateTime StartDateAndTimeOfAppointment { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDateAndTimeOfAppointment { get; set; }

        public bool? Status { get; set; }
        public string Remarks { get; set; }



    }
}