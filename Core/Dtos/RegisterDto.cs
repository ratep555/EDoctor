using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class RegisterDto
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


        [Required, MinLength(3), MaxLength(60)]
        public string Country { get; set; }


        [Required, MinLength(2), MaxLength(40)]
        public string City { get; set; }


        [Required, MinLength(2), MaxLength(40)]
        public string Street { get; set; }
        
        public string MBO { get; set; }
        

        [Required, MinLength(5), MaxLength(25)]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
