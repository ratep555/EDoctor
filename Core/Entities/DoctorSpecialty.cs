namespace Core.Entities
{
    public class DoctorSpecialty
    {
        public int DoctorId { get; set; }       
        public Doctor Doctor { get; set; }
        
        public int SpecialtyId { get; set; }
        public Specialty Specialties { get; set; }
    }
}