using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<UserToReturnDto>> GetAllUsers(QueryParameters queryParameters);
        Task<int> GetCountForUsers();
        Task<ApplicationUser> FindUserById(int id);
        Task LockUser(int id);
        Task UnlockUser(int id);      
        Task<string> GetRoleName(int userId);
        Task UpdateUserProfile(DoctorEditDto doctorDto);
        Task UpdateUserPatientProfile(PatientEditDto patientDto);
    }
}