using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IOfficeRepository
    {
        Task<List<Office>> GetAllOffices(QueryParameters queryParameters);
        Task<int> GetCountForAllOffices();
        Task<List<Office>> GetOfficesForDoctor(QueryParameters queryParameters, int userId);
        Task<int> GetCountForOfficesForDoctor(int userId);
        Task<Office> GetOfficeById(int id);
        Task CreateOffice(Office office);
        Task UpdateOffice(Office office);
        Task<Doctor> FindDoctorByUserId(int userId);
        Task<List<Hospital>> GetHospitalsForDoctor(int userId);


    }
}