using System.Threading.Tasks;
using Core.Dtos;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<string> GetRoleName(int userId);
        Task UpdateUserProfile(DoctorEditDto doctorDto);

    }
}