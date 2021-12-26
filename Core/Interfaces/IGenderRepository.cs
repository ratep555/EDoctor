using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IGenderRepository
    {
        Task<List<Gender>> GetAllGendersForAdminList(QueryParameters queryParameters);
        Task<int> GetCountForAllGendersForAdminList();
        Task<Gender> GetGenderById(int id);
        Task CreateGender(Gender gender);
        Task UpdateGender(Gender gender);
        Task DeleteGender(Gender gender);
    }
}