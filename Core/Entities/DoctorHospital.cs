using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class DoctorHospital : BaseEntity
    {
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }

      
    }
}