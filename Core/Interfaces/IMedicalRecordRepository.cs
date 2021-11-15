using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IMedicalRecordRepository
    {
        Task<List<MedicalRecord>> GetAllMedicalRecordsForDoctorsPatient(int id, int userId, 
            QueryParameters queryParameters);
        Task<int> GetCountForMedicalRecordsForDoctorsPatient(int id, int userId);

    }
}