using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class GenderRepository : IGenderRepository
    {
        private readonly EDoctorContext _context;
        public GenderRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<Gender>> GetAllGendersForAdminList(QueryParameters queryParameters)
        {
            IQueryable<Gender> genders = _context.Genders.AsQueryable().OrderBy(x => x.GenderType);

            if (queryParameters.HasQuery())
            {
                genders = genders.Where(t => t.GenderType.Contains(queryParameters.Query));
            }

            genders = genders.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await genders.ToListAsync();
        }

        public async Task<int> GetCountForAllGendersForAdminList()
        {
            return await _context.Genders.CountAsync();
        }

        public async Task<Gender> GetGenderById(int id)
        {
            return await _context.Genders.FirstOrDefaultAsync(p => p.Id == id);        
        }

        public async Task CreateGender(Gender gender)
        {
            _context.Genders.Add(gender);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdateGender(Gender gender)
        {
            _context.Entry(gender).State = EntityState.Modified;        
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGender(Gender gender)
        {
            _context.Genders.Remove(gender);
            await _context.SaveChangesAsync();
        }   

    }
}