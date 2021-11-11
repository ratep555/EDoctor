using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IDoctorRepository
    {
        Task CreateDoctor(int userId, Doctor doctor, string lastname, string firstname);
        Task<Doctor> FindDoctorByUserId(int userId);

    }
}