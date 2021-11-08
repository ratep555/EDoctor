using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Data.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly EDoctorContext _context;

        public PatientRepository(EDoctorContext context)
        {
            _context = context;
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









