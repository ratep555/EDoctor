using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IDoctorRepository
    {
        Task<List<DoctorDto>> GetAllDoctors(QueryParameters queryParameters);
        Task<int> GetCountForAllDoctors();
        Task<Doctor> FindDoctorById(int id);
        Task<Doctor> FindDoctorByUserId(int userId);
        Task CreateDoctor(int userId, Doctor doctor, string lastname, string firstname);
        Task<List<Office>> GetAllOfficesForDoctorById(int id);
        Task<List<Office>> GetAllOfficesForDoctorByUserId(int userId);
        Task<List<Specialty>> GetNonSelectedSpecialties(List<int> ids);
        Task<List<Hospital>> GetNonSelectedHospitals(List<int> ids);
        Task Save();
    }
}