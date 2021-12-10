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
                users = users.Where(x => x.Username.Contains(queryParameters.Query));
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
            user.PhoneNumber = patientDto.PhoneNumber;

            _context.Entry(user).State = EntityState.Modified;     
        }

        public async Task<StatisticsDto> ShowCountForEntities()
        {
            var statistics = new StatisticsDto();

            statistics.PatientsCount = await _context.Patients.CountAsync();
            statistics.DoctorsCount = await _context.Doctors.CountAsync();
            statistics.OfficesCount = await _context.Offices.CountAsync();
            statistics.AllAppointmentsCount = await _context.Appointments.CountAsync();
            statistics.AppointmentsCount = await _context.Appointments
                .Where(x => x.Status == true && x.PatientId != null
                && x.EndDateAndTimeOfAppointment > DateTime.Now).CountAsync();

            return statistics;
        }

        public async Task<IEnumerable<ChartDto1>> GetNumberAndTypeOfDoctorsForChart()
        {
            List<ChartDto1> list = new List<ChartDto1>();

            var alloffices = await _context.Offices.ToListAsync();

            IEnumerable<int> ids = alloffices.Select(x => x.DoctorId);

            var hospitaloffices = await _context.Offices.Where(x => x.HospitalId != null).ToListAsync();

            IEnumerable<int> ids1 = hospitaloffices.Select(x => x.DoctorId);

            var nonhospitaloffices = await _context.Offices.Where(x => x.HospitalId == null).ToListAsync();

            IEnumerable<int> ids2 = nonhospitaloffices.Select(x => x.DoctorId);

            list.Add(new ChartDto1 { DoctorType = "Hospital and private doctors", 
                NumberOfDoctors = await _context.Doctors.Where(p => ids.Contains(p.Id) && ids1.Contains(p.Id)
                && ids2.Contains(p.Id)).CountAsync()  });

            list.Add(new ChartDto1 { DoctorType = "Hospital doctors", 
                NumberOfDoctors = await _context.Doctors.Where(p => ids.Contains(p.Id) 
                && ids1.Contains(p.Id) && !ids2.Contains(p.Id)).CountAsync()  });

            list.Add(new ChartDto1 { DoctorType = "Private doctors", 
                NumberOfDoctors = await _context.Doctors.Where(p => ids.Contains(p.Id) 
                && !ids1.Contains(p.Id) && ids2.Contains(p.Id)).CountAsync()  });        

            return list;
        }

        public async Task<IEnumerable<ChartDto2>> GetNumberAndTypeOfOfficesForChart()
        {
            List<ChartDto2> list = new List<ChartDto2>();

            list.Add(new ChartDto2 { OfficeType = "Hospital offices", 
                NumberOfOffices = await _context.Offices.Where(x => x.HospitalId != null).CountAsync()  });

            list.Add(new ChartDto2 { OfficeType = "Private offices", 
                NumberOfOffices = await _context.Offices.Where(x => x.HospitalId == null).CountAsync()  });
            
            return list;
        }

        public async Task<IEnumerable<ChartDto3>> GetNumberAndTypeOfAppointmentsForChart()
        {
            List<ChartDto3> list = new List<ChartDto3>();

            list.Add(new ChartDto3 { AppointmentType = "Previous", 
                NumberOfAppointments = await _context.Appointments
                .Where(x => x.EndDateAndTimeOfAppointment < DateTime.Now && x.Status == true).CountAsync()  });

            list.Add(new ChartDto3 { AppointmentType = "Upcoming", 
                NumberOfAppointments = await _context.Appointments
                .Where(x => x.Status == true && x.PatientId != null
                && x.EndDateAndTimeOfAppointment > DateTime.Now).CountAsync()  });

            list.Add(new ChartDto3 { AppointmentType = "Available", 
                NumberOfAppointments = await _context.Appointments.Include(x => x.Office)
                .Where(x => x.Status == null && x.PatientId == null 
                && x.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync()  });
            
            return list;
        }
 
        public async Task<IEnumerable<ChartDto4>> GetNumberAndTypeOfPatientsForChart()
        {
            List<ChartDto4> list = new List<ChartDto4>();

            list.Add(new ChartDto4 { PatientType = "Patients with MBO", 
                NumberOfPatients = await _context.Patients.Where(x => x.MBO != null).CountAsync()  });

            list.Add(new ChartDto4 { PatientType = "Patients without MBO", 
                NumberOfPatients = await _context.Patients.Where(x => x.MBO == null).CountAsync()  });
            
            return list;
        }

         public async Task<IEnumerable<ChartDto5>> GetNumberAndTypeOfPatientsGenderForChart()
        {
            List<ChartDto5> list = new List<ChartDto5>();

            list.Add(new ChartDto5 { GenderType = "Female", 
                NumberOfPatients = await _context.Patients.Include(x => x.Gender)
                .Where(x => x.Gender.GenderType == "Female").CountAsync()  });

            list.Add(new ChartDto5 { GenderType = "Male", 
                NumberOfPatients = await _context.Patients.Include(x => x.Gender)
                .Where(x => x.Gender.GenderType == "Male").CountAsync()  });

            list.Add(new ChartDto5 { GenderType = "Other", 
                NumberOfPatients = await _context.Patients.Include(x => x.Gender)
                .Where(x => x.Gender.GenderType == "Other").CountAsync()  });
            
            return list;
        }
    }
}



