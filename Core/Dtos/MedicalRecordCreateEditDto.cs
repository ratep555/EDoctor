using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class MedicalRecordCreateEditDto
    {
        public int AppointmentId { get; set; }
        public string AnamnesisDiagnosisTherapy { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        
        public int PatientId { get; set; }
        public int OfficeId { get; set; }     
    }
}