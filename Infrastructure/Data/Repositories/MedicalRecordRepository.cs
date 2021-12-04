using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly EDoctorContext _context;
        public MedicalRecordRepository(EDoctorContext context)
        {
            _context = context;

        }

        public async Task<List<MedicalRecord>> GetMedicalRecordsForAllPatientsOfDoctor(int userId, 
            QueryParameters queryParameters)
        {       
            IQueryable<MedicalRecord> records = _context.MedicalRecords
                                            .Include(x => x.Patient)
                                            .Include(x => x.Office).ThenInclude(x => x.Doctor)
                                            .Include(x => x.Office).ThenInclude(x => x.Hospitals)
                                            .Where(x => x.Office.Doctor.ApplicationUserId == userId)
                                            .AsQueryable().OrderByDescending(x => x.Created);

            var office = await _context.Offices.Include(x => x.MedicalRecords).Include(x => x.Doctor)
                          .Where(x => x.Doctor.ApplicationUserId == userId && x.Id == queryParameters.OfficeId) 
                          .FirstOrDefaultAsync();
            
            if (queryParameters.HasQuery())
            {
                records = records
                .Where(x => x.Patient.Name.Contains(queryParameters.Query));
            }

            if (queryParameters.OfficeId.HasValue)
            {
                records = records.Where(x => x.OfficeId == office.Id);
            }

            records = records.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                           .Take(queryParameters.PageCount);
            
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                var hospitaloffices = await _context.Offices.Where(x => x.HospitalId != null).ToListAsync();

                IEnumerable<int> ids1 = hospitaloffices.Select(x => x.DoctorId);

                var nonhospitaloffices = await _context.Offices.Where(x => x.HospitalId == null).ToListAsync();

                IEnumerable<int> ids2 = nonhospitaloffices.Select(x => x.DoctorId);

                switch (queryParameters.Sort)
                {
                    case "dateAsc":
                        records = records.OrderBy(p => p.Created);
                        break;
                    case "hospital":
                        records = records
                        .Where(p => ids1.Contains(p.OfficeId));
                        break;
                    case "private":
                        records = records
                        .Where(p => !ids1.Contains(p.OfficeId) && ids2.Contains(p.OfficeId));
                        break;
                    default:
                        records = records.OrderByDescending(n => n.Created);
                        break;
                }
            }    
            return await records.ToListAsync();        
        }

        public async Task<int> GetCountForMedicalRecordsForAllPatientsOfDoctor(int userId)
        {
            return await _context.MedicalRecords.Include(x => x.Office).ThenInclude(x => x.Doctor) 
                         .Where(x => x.Office.Doctor.ApplicationUserId == userId)
                         .CountAsync();
        }

        public async Task<List<MedicalRecord>> GetAllMedicalRecordsForPatientAsUser(int userId, 
            QueryParameters queryParameters)
        {       
            IQueryable<MedicalRecord> records = _context.MedicalRecords
                                                .Include(x => x.Patient)
                                                .Include(x => x.Office).ThenInclude(x => x.Doctor)
                                                .Include(x => x.Office).ThenInclude(x => x.Hospitals)
                                                .Where(x => x.Patient.ApplicationUserId == userId)
                                                .AsQueryable().OrderByDescending(x => x.Created);
            
            if (queryParameters.HasQuery())
            {
                records = records
                .Where(x => x.Office.Doctor.Name.Contains(queryParameters.Query));
            }

            records = records.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                           .Take(queryParameters.PageCount);
            
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "dateAsc":
                        records = records.OrderBy(p => p.Created);
                        break;
                    default:
                        records = records.OrderByDescending(n => n.Created);
                        break;
                }
            }    
            return await records.ToListAsync();        
        }

        public async Task<int> GetCountForAllMedicalRecordsForPatientAsUser(int userId)
        {
            return await _context.MedicalRecords.Include(x => x.Patient)
                         .Where(x => x.Patient.ApplicationUserId == userId)
                         .CountAsync();
        }

        public async Task<List<MedicalRecord>> GetMedicalRecordsForOnePatientOfDoctor(int id, int userId, 
            QueryParameters queryParameters)
        {
            IQueryable<MedicalRecord> records = _context.MedicalRecords
                        .Include(x => x.Patient).Include(x => x.Office)
                        .Where(x => x.PatientId == id && x.Office.Doctor.ApplicationUserId == userId)
                        .AsQueryable().OrderBy(x => x.Created);
            
            if (queryParameters.HasQuery())
            {
                records = records
                .Where(x => x.Office.Street.Contains(queryParameters.Query));
            }

            records = records.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                           .Take(queryParameters.PageCount);
            
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "dateAsc":
                        records = records.OrderBy(p => p.Created);
                        break;
                    default:
                        records = records.OrderByDescending(n => n.Created);
                        break;
                }
            }               
            return await records.ToListAsync();        
        }

        public async Task<int> GetCountForMedicalRecordsForOnePatientOfDoctor(int id, int userId)
        {
            var doctor = await _context.Doctors.Where(x => x.ApplicationUserId == userId).FirstOrDefaultAsync();

            var offices = await _context.Offices.Where(x => x.DoctorId == doctor.Id).ToListAsync();
           
            IEnumerable<int> ids = offices.Select(x => x.Id).ToList();

            return await _context.MedicalRecords.Where(x => x.PatientId == id && ids.Contains(x.OfficeId))
                         .CountAsync();
        }

        public async Task<MedicalRecord> FindMedicalRecordById(int id)
        {
            return await _context.MedicalRecords.Include(x => x.Patient)
                         .Include(x => x.Office).ThenInclude(x => x.Doctor)
                         .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateMedicalRecord(MedicalRecord medicalRecord)
        {
            medicalRecord.Created = medicalRecord.Created.ToLocalTime();
            
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();
        }


    }
}












