using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EDoctorContext _context;

        public UserRepository(EDoctorContext context)
        {
            _context = context;
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
    }
}









