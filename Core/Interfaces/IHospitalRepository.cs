using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IHospitalRepository
    {
        Task<List<Hospital>> GetAllHospitalsForAdminList(QueryParameters queryParameters);
        Task<int> GetCountForAllHospitalsForAdminList();  
        Task<List<Hospital>> GetAllHospitalsForOffice();
        Task<Hospital> GetHospitalById(int id);
        Task CreateHospital(Hospital hospital);
        Task UpdateHospital(Hospital hospital);     
        Task DeleteHospital(Hospital hospital);   
    }
}