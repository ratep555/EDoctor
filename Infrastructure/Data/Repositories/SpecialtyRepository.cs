using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace Infrastructure.Data.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly EDoctorContext _context;
        public SpecialtyRepository(EDoctorContext context)
        {
            _context = context;

        }
        public async Task<List<Specialty>> GetSpecialties()
        {
            return await _context.Specialties.ToListAsync();
        }
    }
}