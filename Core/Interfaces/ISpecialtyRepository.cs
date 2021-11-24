using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface ISpecialtyRepository
    {
        Task<List<Specialty>> GetAllSpecialtiesForAdminList(QueryParameters queryParameters);
        Task<int> GetCountForAllSpecialtieForAdminList();     
        Task<List<Specialty>> GetAllSpecialties();
        Task<List<Specialty>> GetSpecialtiesAttributedToDoctors();
        Task<Specialty> GetSpecialtyById(int id);
        Task CreateSpecialty(Specialty specialty);
        Task UpdateSpecialty(Specialty specialty);
        Task DeleteSpecialty(Specialty specialty);
    }
}