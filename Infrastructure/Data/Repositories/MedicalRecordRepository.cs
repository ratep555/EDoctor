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

        public async Task<List<MedicalRecord>> GetAllMedicalRecordsForDoctorsPatient(int id, int userId, 
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

        public async Task<int> GetCountForMedicalRecordsForDoctorsPatient(int id, int userId)
        {
            var doctor = await _context.Doctors.Where(x => x.ApplicationUserId == userId).FirstOrDefaultAsync();

            var offices = await _context.Offices.Where(x => x.DoctorId == doctor.Id).ToListAsync();
           
            IEnumerable<int> ids = offices.Select(x => x.Id).ToList();

            return await _context.MedicalRecords.Where(x => x.PatientId == id && ids.Contains(x.OfficeId))
                         .CountAsync();
        }


    }
}