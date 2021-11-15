using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Patient : BaseEntity
    {
        public int ApplicationUserId { get; set; }     

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }   

        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
