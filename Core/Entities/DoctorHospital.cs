using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class DoctorHospital
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }

      
    }
}