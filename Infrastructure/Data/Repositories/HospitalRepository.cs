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
    public class HospitalRepository : IHospitalRepository
    {
        private readonly EDoctorContext _context;

        public HospitalRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<Hospital>> GetAllHospitalsForAdminList(QueryParameters queryParameters)
        {
            IQueryable<Hospital> hospitals = _context.Hospitals.AsQueryable().OrderBy(x => x.HospitalName);
            
            if (queryParameters.HasQuery())
            {
                hospitals = hospitals.Where(t => t.HospitalName.Contains(queryParameters.Query));
            }

            hospitals = hospitals.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
            
            return await hospitals.ToListAsync();        
        }

        public async Task<int> GetCountForAllHospitalsForAdminList()
        {
            return await _context.Hospitals.CountAsync();
        }

        public async Task<List<Hospital>> GetAllHospitalsForOffice()
        {
            return await _context.Hospitals.OrderBy(x => x.HospitalName).ToListAsync();
        }

        public async Task<Hospital> GetHospitalById(int id)
        {
            return await _context.Hospitals.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateHospital(Hospital hospital)
        {
            _context.Hospitals.Add(hospital);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdateHospital(Hospital hospital)
        {
            _context.Entry(hospital).State = EntityState.Modified;        
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHospital(Hospital hospital)
        {
            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();
        }
    }
}







