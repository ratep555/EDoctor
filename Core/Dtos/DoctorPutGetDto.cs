using System.Collections.Generic;

namespace Core.Dtos
{
    public class DoctorPutGetDto
    {
        public DoctorDto Doctor { get; set; }
        public List<SpecialtyDto> SelectedSpecialties { get; set; }
        public List<SpecialtyDto> NonSelectedSpecialties { get; set; }
        public List<HospitalDto> SelectedHospitals { get; set; }
        public List<HospitalDto> NonSelectedHospitals { get; set; }
    }
}