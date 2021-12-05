using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IMedicalRecordRepository
    {
        Task<List<MedicalRecord>> GetMedicalRecordsForAllPatientsOfDoctor(int userId, 
            QueryParameters queryParameters);
        Task<int> GetCountForMedicalRecordsForAllPatientsOfDoctor(int userId);
        Task<List<MedicalRecord>> GetAllMedicalRecordsForPatientAsUser(int userId, QueryParameters queryParameters);
        Task<int> GetCountForAllMedicalRecordsForPatientAsUser(int userId);
        Task<List<MedicalRecord>> GetMedicalRecordsForOnePatientOfDoctor(int id, int userId, 
            QueryParameters queryParameters);
        Task<int> GetCountForMedicalRecordsForOnePatientOfDoctor(int id, int userId);
        Task<MedicalRecord> FindMedicalRecordById(int id);
        Task CreateMedicalRecord(MedicalRecord medicalRecord);
        Task UpdateMedicalRecord(MedicalRecord record);
    }
}