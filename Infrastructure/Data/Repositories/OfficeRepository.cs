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
    public class OfficeRepository : IOfficeRepository
    {
        private readonly EDoctorContext _context;

        public OfficeRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<Office>> GetAllOffices(QueryParameters queryParameters)
        {
                                 
            var doctorSpecialty = await _context.DoctorSpecialties.
                                        Where(x => x.SpecialtyId == queryParameters.SpecialtyId) 
                                        .FirstOrDefaultAsync();

            IQueryable<Office> offices = _context.Offices.Include(x => x.Doctor)
                                        .ThenInclude(x => x.DoctorSpecialties).ThenInclude(x => x.Doctor)
                                        .AsQueryable().OrderBy(x => x.City);
            
            if (queryParameters.HasQuery())
            {
                offices = offices
                .Where(x => x.City.Contains(queryParameters.Query)
                || x.Doctor.Name.Contains(queryParameters.Query));
            }

            if (queryParameters.SpecialtyId.HasValue)
            {
                offices = offices.Where(x => x.DoctorId == doctorSpecialty.DoctorId);

            }

            offices = offices.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                             .Take(queryParameters.PageCount);

            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "priceAsc":
                        offices = offices.OrderBy(p => p.InitialExaminationFee);
                        break;
                    case "priceDesc":
                        offices = offices.OrderByDescending(p => p.InitialExaminationFee);
                        break;
                    case "priceAscFollowUp":
                        offices = offices.OrderBy(p => p.FollowUpExaminationFee);
                        break;
                    case "priceDescFollowUp":
                        offices = offices.OrderByDescending(p => p.FollowUpExaminationFee);
                        break;
                    default:
                        offices = offices.OrderBy(n => n.City);
                        break;
                }
            }           
            return await offices.ToListAsync();        
        }

        public async Task<int> GetCountForAllOffices()
        {
            return await _context.Offices.CountAsync();
        }

        public async Task<List<Office>> GetOfficesForDoctor(QueryParameters queryParameters, int userId)
        {
            var doctor = await _context.Doctors.Where(x => x.ApplicationUserId == userId)
                               .FirstOrDefaultAsync();

            IQueryable<Office> office = _context.Offices.Where(x => x.DoctorId == doctor.Id)
                                        .AsQueryable().OrderBy(x => x.City);
            
            if (queryParameters.HasQuery())
            {
                office = office.Where(x => x.City.Contains(queryParameters.Query));
            }

            office = office.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                           .Take(queryParameters.PageCount);
            
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "priceAsc":
                        office = office.OrderBy(p => p.InitialExaminationFee);
                        break;
                    case "priceDesc":
                        office = office.OrderByDescending(p => p.InitialExaminationFee);
                        break;
                    default:
                        office = office.OrderBy(n => n.City);
                        break;
                }
            }             
            return await office.ToListAsync();        
        }

        public async Task<int> GetCountForOfficesForDoctor(int userId)
        {
            return await _context.Offices.Include(x => x.Doctor)
                         .Where(x => x.Doctor.ApplicationUserId == userId).CountAsync();
        }

        public async Task<Office> GetOfficeById(int id)
        {
            return await _context.Offices.Include(x => x.Doctor).Where(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task CreateOffice(Office office)
        {
            _context.Offices.Add(office);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOffice(Office office)
        {
            _context.Entry(office).State = EntityState.Modified;        
            await _context.SaveChangesAsync();
        }

        public async Task<Doctor> FindDoctorByUserId(int userId)
        {
            return await _context.Doctors.Where(x => x.ApplicationUserId == userId)
                         .FirstOrDefaultAsync();
        }


        public async Task<List<Hospital>> GetHospitalsForDoctor(int userId)
        {
            var doctorhospitals = await _context.DoctorHospitals
                                  .Where(x => x.Doctor.ApplicationUserId == userId)
                                  .ToListAsync();

            IEnumerable<int> ids = doctorhospitals.Select(x => x.HospitalId);

            var hospitals = await _context.Hospitals.Where(x => ids.Contains(x.Id)).ToListAsync();

            return hospitals;
        }


    }
}










