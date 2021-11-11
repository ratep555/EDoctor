using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly EDoctorContext _context;

        public SpecialtyRepository(EDoctorContext context)
        {
            _context = context;
        }
        public async Task<List<Specialty>> GetAllSpecialties()
        {
            return await _context.Specialties.OrderBy(x => x.SpecialtyName).ToListAsync();
        }

        public async Task<List<Specialty>> GetSpecialtiesAttributedToDoctors()
        {
            var doctorSpecialties = await _context.DoctorSpecialties.ToListAsync();

            IEnumerable<int> ids = doctorSpecialties.Select(x => x.SpecialtyId);

            var specialties = await _context.Specialties.Where(x => ids.Contains(x.Id))
                                    .OrderBy(x => x.SpecialtyName).ToListAsync();

            return specialties;
        }

    }
}





