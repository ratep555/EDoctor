using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class DoctorCreateDto
    {
        [Required, MinLength(2), MaxLength(30)]
        public string FirstName { get; set; }


        [Required, MinLength(2), MaxLength(60)]
        public string LastName { get; set; }

        
        [Required, MinLength(2), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        [MaxLength(5000)]
        public string Resume { get; set; }

        [MaxLength(1000)]
        public string Qualifications { get; set; }


        [DataType(DataType.Date)]
        public DateTime? StartedPracticing { get; set; }


        public List<int> SpecialtiesIds { get; set; }
        public List<int> HospitalsIds { get; set; }
    }
}



