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
        Task<List<Gender>> GetGendersForPatient();
        Task CreatePatient(ApplicationUser user, RegisterDto registerDto);
        Task Save();
        Task<PatientStatisticsDto> ShowCountForEntitiesForPatient(int userId);
        Task<IEnumerable<PatientChartDto1>> GetNumberAndTypeOfMedicalRecordsForPatientForChart(int userId);
        Task<IEnumerable<PatientChartDto2>> GetNumberAndTypeOfOfficesForPatientForChart(int userId);
        Task<IEnumerable<PatientChartDto3>> GetNumberAndTypeOfAppointmentsForPatientForChart(int userId);
    }
}