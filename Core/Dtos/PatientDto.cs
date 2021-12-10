using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class PatientDto
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }     
        public int GenderId { get; set; }   
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string MBO { get; set; }
    }
}