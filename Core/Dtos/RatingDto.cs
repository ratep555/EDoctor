using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class RatingDto
    {
        [Range(1, 5)]
        public int Rating { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}