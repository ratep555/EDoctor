using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class MedicalRecord
    {
        [Key, ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        public string AnamnesisDiagnosisTherapy { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public Appointment Appointment { get; set; }


    }
}