using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> FindPatientByUserId(int userId);
        Task CreatePatient(int userId, string lastname, string firstname, DateTime dateOfBirth);

    }
}