using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
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

        public async Task<List<Specialty>> GetAllSpecialtiesForAdminList(QueryParameters queryParameters)
        {
            IQueryable<Specialty> specialties = _context.Specialties.AsQueryable().OrderBy(x => x.SpecialtyName);
            
            if (queryParameters.HasQuery())
            {
                specialties = specialties.Where(t => t.SpecialtyName.Contains(queryParameters.Query));
            }

            specialties = specialties.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
            
            return await specialties.ToListAsync();        
        }

        public async Task<int> GetCountForAllSpecialtieForAdminList()
        {
            return await _context.Specialties.CountAsync();
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

        public async Task<Specialty> GetSpecialtyById(int id)
        {
            return await _context.Specialties.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateSpecialty(Specialty specialty)
        {
            _context.Specialties.Add(specialty);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdateSpecialty(Specialty specialty)
        {
            _context.Entry(specialty).State = EntityState.Modified;        
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpecialty(Specialty specialty)
        {
            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();
        }

    }
}





