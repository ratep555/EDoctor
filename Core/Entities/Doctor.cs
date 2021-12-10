using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Doctor : BaseEntity
    {
        public int ApplicationUserId { get; set; }     

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }   
        
        public string Name { get; set; }
        public string Resume { get; set; }
        public string Picture { get; set; }
        public string Qualifications { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime? StartedPracticing { get; set; }
        
        public ICollection<Office> Offices { get; set; }
        public ICollection<Rating> Ratings { get; set; }       
        public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; }
        public ICollection<DoctorHospital> DoctorHospitals { get; set; }
  
    }
}