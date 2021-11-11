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
    public class HospitalRepository : IHospitalRepository
    {
        private readonly EDoctorContext _context;

        public HospitalRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<Hospital>> GetAllHospitals()
        {
            return await _context.Hospitals.OrderBy(x => x.HospitalName).ToListAsync();
        }
    }
}