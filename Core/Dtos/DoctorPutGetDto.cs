using System.Collections.Generic;

namespace Core.Dtos
{
    public class DoctorPutGetDto
    {
        public DoctorDto Doctor { get; set; }
        public IEnumerable<SpecialtyDto> SelectedSpecialties { get; set; }
        public IEnumerable<SpecialtyDto> NonSelectedSpecialties { get; set; }
        public IEnumerable<HospitalDto> SelectedHospitals { get; set; }
        public IEnumerable<HospitalDto> NonSelectedHospitals { get; set; }
    }
}