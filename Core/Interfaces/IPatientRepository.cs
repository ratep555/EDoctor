using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPatientRepository
    {
        Task CreatePatient(int userId, string lastname, string firstname, DateTime dateOfBirth);

    }
}