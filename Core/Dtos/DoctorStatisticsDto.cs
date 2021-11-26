namespace Core.Dtos
{
    public class DoctorStatisticsDto
    {
        public int UpcomingAppointmentsCount { get; set; }
        public int AvailableAppointmentsCount { get; set; }
        public int MedicalRecordsCount { get; set; }
        public int PatientsCount { get; set; }
    }
}