using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Patient> FindPatientByUserId(int userId)
        {
            return await _context.Patients.Where(x => x.ApplicationUserId == userId)
                         .FirstOrDefaultAsync();
        }

        public async Task<Patient> FindPatientById(int id)
        {
            return await _context.Patients.Include(x => x.ApplicationUser)
                         .Where(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task CreatePatient(int userId, string lastname, string firstname, DateTime dateOfBirth)
        {
            var patient = new Patient();
            patient.ApplicationUserId = userId;
            patient.Name = string.Format("{0} {1}", lastname, firstname);
            patient.DateOfBirth = dateOfBirth;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }
    }
}









