using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Patient : BaseEntity
    {
        public int ApplicationUserId { get; set; }     

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }   

        public int GenderId { get; set; }   
          
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }   

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string MBO { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
