namespace Core.Dtos
{
    public class PatientStatisticsDto
    {
        public int AppointmentsCount { get; set; }
        public int DoctorsCount { get; set; }
        public int MedicalRecordsCount { get; set; }
        public int OfficesCount { get; set; }
    }
}