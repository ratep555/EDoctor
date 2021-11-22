using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllPatientsOfDoctor(int userId, QueryParameters queryParameters);
        Task<int> GetCountForAllPatientsOfDoctor(int userId);
        Task<Patient> FindPatientById(int id);
        Task<Patient> FindPatientByUserId(int userId);
        Task CreatePatient(ApplicationUser user, RegisterDto registerDto);
        Task Save();
    }
}