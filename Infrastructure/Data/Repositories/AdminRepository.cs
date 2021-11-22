using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly EDoctorContext _context;

        public AdminRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<UserToReturnDto>> GetAllUsers(QueryParameters queryParameters)
        {
            IQueryable<UserToReturnDto> users = (from u in _context.Users
                                               select new UserToReturnDto 
                                               {
                                                   Username = u.UserName,
                                                   Email = u.Email,
                                                   UserId = u.Id,
                                                   LockoutEnd = u.LockoutEnd,
                                                   Roles = u.UserRoles.Select(r => r.Role.Name).ToList()

                                            }).AsQueryable().OrderBy(x => x.Username);
            
            if (queryParameters.HasQuery())
            {
                users = users
                .Where(x => x.Username.Contains(queryParameters.Query));
            }

            users = users.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                           .Take(queryParameters.PageCount);
            
            return await users.ToListAsync();       
        }

        public async Task<int> GetCountForUsers()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<ApplicationUser> FindUserById(int id)
        {
            return await _context.Users.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task LockUser(int id)
        {
            var userFromDb = await FindUserById(id);

            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);

            await _context.SaveChangesAsync();
        }

        public async Task UnlockUser(int id)
        {
            var userFromDb = await FindUserById(id);

            userFromDb.LockoutEnd = null;

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetRoleName(int userId)
        {
            var roleName = await (from r in _context.Roles
                                  join ur in _context.UserRoles
                                  on r.Id equals ur.RoleId
                                  join u in _context.Users.Where(u => u.Id == userId)
                                  on ur.UserId equals u.Id
                                  select r.Name
                                 ).FirstOrDefaultAsync();

            return roleName;
        }

        public async Task UpdateUserProfile(DoctorEditDto doctorDto)
        {
            var user = await _context.Users.Where(x => x.Id == doctorDto.ApplicationUserId).FirstOrDefaultAsync();

            string[] fullname = doctorDto.Name.Split(' ');
            user.LastName = fullname[0];
            user.FirstName = fullname[1];

            _context.Entry(user).State = EntityState.Modified;     
        }

        public async Task UpdateUserPatientProfile(PatientEditDto patientDto)
        {
            var user = await _context.Users.Where(x => x.Id == patientDto.ApplicationUserId).FirstOrDefaultAsync();

            string[] fullname = patientDto.Name.Split(' ');
            user.LastName = fullname[0];
            user.FirstName = fullname[1];

            _context.Entry(user).State = EntityState.Modified;     
        }
    }
}



