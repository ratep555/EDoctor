using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }            
        public string Name { get; set; }
        public string Resume { get; set; }
        public int? RateSum { get; set; }
        public int? Count { get; set; }
        public double? AverageVote { get; set; }
        public int UserVote { get; set; }
        public string Picture { get; set; }
        public string Qualifications { get; set; }
        
        
        [DataType(DataType.Date)]
        public DateTime? StartedPracticing { get; set; }
  
        public List<SpecialtyDto> Specialties { get; set; }
        public List<HospitalDto> Hospitals { get; set; }        
        public List<OfficeDto> Offices { get; set; }        
    }
}


