using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly EDoctorContext _context;

        public PatientRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllPatientsOfDoctor(int userId,
            QueryParameters queryParameters)
        {   
            var medicalrecords = await _context.MedicalRecords.Include(x => x.Office).ThenInclude(x => x.Doctor)
                                 .Where(x => x.Office.Doctor.ApplicationUserId == userId).ToListAsync();

            IEnumerable<int> ids = medicalrecords.Select(x => x.PatientId);

            IQueryable<Patient> patients =  _context.Patients.Include(x => x.ApplicationUser)
                                            .Where(x => ids.Contains(x.Id))
                                            .AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                patients = patients
                .Where(x => x.Name.Contains(queryParameters.Query));
            }

            patients = patients.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                               .Take(queryParameters.PageCount);
            
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "nameDesc":
                        patients = patients.OrderByDescending(p => p.Name);
                        break;
                    default:
                        patients = patients.OrderBy(n => n.Name);
                        break;
                }
            }     
            return await patients.ToListAsync();        
        }

        public async Task<int> GetCountForAllPatientsOfDoctor(int userId)
        {
            var medicalrecords = await _context.MedicalRecords.Include(x => x.Office).ThenInclude(x => x.Doctor)
                                 .Where(x => x.Office.Doctor.ApplicationUserId == userId).ToListAsync();

            IEnumerable<int> ids = medicalrecords.Select(x => x.PatientId);

            return await _context.Patients.Where(x => ids.Contains(x.Id)).CountAsync();
        }

        public async Task<Patient> FindPatientByUserId(int userId)
        {
            return await _context.Patients.Include(x => x.ApplicationUser)
                         .Where(x => x.ApplicationUserId == userId)
                         .FirstOrDefaultAsync();
        }

        public async Task<Patient> FindPatientById(int id)
        {
            return await _context.Patients.Include(x => x.ApplicationUser)
                         .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreatePatient(ApplicationUser user, RegisterDto registerDto)
        {
            var patient = new Patient();
            patient.ApplicationUserId = user.Id;
            patient.Name = string.Format("{0} {1}", user.FirstName, user.LastName);
            patient.DateOfBirth = registerDto.DateOfBirth;
            patient.Street = registerDto.Street;
            patient.City = registerDto.City;
            patient.Country = registerDto.Country;
            patient.MBO = registerDto.MBO;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}









