using System.Collections.Generic;

namespace Core.Entities
{
    public class Hospital : BaseEntity
    {
        public string HospitalName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<DoctorHospital> DoctorHospitals { get; set; }

    }
}