using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class DoctorCreateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        public string Resume { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartedPracticing { get; set; }


        public List<int> SpecialtiesIds { get; set; }
    }
}