using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRatingRepository
    {
        Task<Rating> FindCurrentRate(int doctorId, int userId);
        Task AddRating(RatingDto ratingDto, int userId);
        Task<bool> CheckIfThisIsDoctorsPatient(int id, int userId);
        Task<bool> ChechIfAny(int id);
        Task<double> AverageVote(int id);
        Task Save();
    }
}