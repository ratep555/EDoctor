using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly EDoctorContext _context;

        public DoctorRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task CreateDoctor(int userId, Doctor doctor, string lastname, string firstname)
        {
            doctor.ApplicationUserId = userId;
            doctor.Name = string.Format("{0} {1}", lastname, firstname);

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<Doctor> FindDoctorByUserId(int userId)
        {
            return await _context.Doctors.Where(x => x.ApplicationUserId == userId)
                         .FirstOrDefaultAsync();
        }

    }
}









