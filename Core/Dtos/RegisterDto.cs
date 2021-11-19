using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class RegisterDto
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
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }
        
        public string MBO { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

    }
}
