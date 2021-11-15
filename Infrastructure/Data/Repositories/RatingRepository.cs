using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly EDoctorContext _context;
        public RatingRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<Rating> FindCurrentRate(int doctorId, int userId)
        {
            return await _context.Ratings.Include(x => x.Patient)
                         .Where(x => x.DoctorId == doctorId && x.Patient.ApplicationUserId == userId).FirstOrDefaultAsync();
        }

        public async Task AddRating(RatingDto ratingDto, int userId)
        {
            var patient = await _context.Patients.Where(x => x.ApplicationUserId == userId)
                                .FirstOrDefaultAsync();

            var rating = new Rating();
            rating.DoctorId = ratingDto.DoctorId;
            rating.Rate = ratingDto.Rating;
            rating.PatientId = patient.Id;

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfThisIsDoctorsPatient(int id, int userId)
        {
            var records = await _context.MedicalRecords.Include(x => x.Office).ThenInclude(x => x.Doctor)
                .Include(x => x.Patient)
                .Where(x => x.Office.Doctor.Id == id && x.Patient.ApplicationUserId == userId).ToListAsync();

           if (!records.Any())
           {
               return true;
           }
           return false;
        }

        public async Task<bool> ChechIfAny(int id)
        {
           return await _context.Ratings.AnyAsync(x => x.DoctorId == id);          
        }

        public async Task<double> AverageVote(int id)
        {
            var average = await _context.Ratings.Where(x => x.DoctorId == id).AverageAsync(x => x.Rate);

            return average;
        }

    }
}








