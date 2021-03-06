using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Core.Entities
{
    public class Office : BaseEntity
    {
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public Hospital Hospitals { get; set; }
        
        public decimal InitialExaminationFee { get; set; }
        public decimal FollowUpExaminationFee { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public Point Location { get; set; }
        public string Picture { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}