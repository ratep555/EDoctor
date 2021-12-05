using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly EDoctorContext _context;

        public DoctorRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorDto>> GetAllDoctors(QueryParameters queryParameters)
        {                             
            var doctorSpecialty = await _context.DoctorSpecialties.
                                        Where(x => x.SpecialtyId == queryParameters.SpecialtyId) 
                                        .FirstOrDefaultAsync();

            IQueryable<DoctorDto> doctors = (from d in _context.Doctors
                        select new DoctorDto
                        {
                            Id = d.Id,
                            Name = d.Name,
                            StartedPracticing = d.StartedPracticing,
                            RateSum = _context.Ratings.Where(x => x.DoctorId == d.Id).Sum(x => x.Rate),
                            Count = _context.Ratings.Where(x => x.DoctorId == d.Id).Count()
                        }).AsQueryable().OrderBy(x => x.Name);
            
            if (queryParameters.HasQuery())
            {
                doctors = doctors
                .Where(x => x.Name.Contains(queryParameters.Query));
            }

            if (queryParameters.SpecialtyId.HasValue)
            {
                doctors = doctors.Where(x => x.Id == doctorSpecialty.DoctorId);
            }

            doctors = doctors.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                           .Take(queryParameters.PageCount);

            var list = await doctors.ToListAsync();
            
            foreach (var item in list)
            {
                if (item.RateSum.HasValue && item.RateSum > 0)
                {
                    item.AverageVote = await _context.Ratings.Where(x => x.DoctorId == item.Id)
                                       .AverageAsync(x => x.Rate);
                }
                else
                {
                    item.AverageVote = 0;
                }
            }

            var listy = list.AsEnumerable();

            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                var alloffices = await _context.Offices.ToListAsync();

                IEnumerable<int> ids = alloffices.Select(x => x.DoctorId);

                var hospitaloffices = await _context.Offices.Where(x => x.HospitalId != null).ToListAsync();

                IEnumerable<int> ids1 = hospitaloffices.Select(x => x.DoctorId);

                var nonhospitaloffices = await _context.Offices.Where(x => x.HospitalId == null).ToListAsync();

                IEnumerable<int> ids2 = nonhospitaloffices.Select(x => x.DoctorId);

                switch (queryParameters.Sort)
                {
                    case "rateDesc":
                        listy = listy.OrderByDescending(p => p.AverageVote);
                        break; 
                    case "rateAsc":
                        listy = listy.OrderBy(p => p.AverageVote);
                        break;
                    case "practicingAsc":
                        listy = listy.OrderBy(p => p.StartedPracticing);
                        break;
                    case "hospitalandprivate":
                        listy = listy.Where(p => ids.Contains(p.Id) && ids1.Contains(p.Id) && ids2.Contains(p.Id));
                        break;
                    case "hospital":
                        listy = listy.Where(p => ids.Contains(p.Id) && ids1.Contains(p.Id) && !ids2.Contains(p.Id));
                        break;
                    case "private":
                        listy = listy.Where(p => ids.Contains(p.Id) && !ids1.Contains(p.Id) && ids2.Contains(p.Id));
                        break;
                    default:
                        listy = listy.OrderBy(n => n.Name);
                        break;
                }
            }  
            return listy.ToList();        
        }

        public async Task<int> GetCountForAllDoctors()
        {
            return await _context.Doctors.CountAsync();
        }

        public async Task<List<DoctorDto>> GetAllDoctorsOfPatient(int userId, QueryParameters queryParameters)
        {
            var medicalrecords = await _context.MedicalRecords.Include(x => x.Appointment)
                .ThenInclude(x => x.Patient).Include(x => x.Appointment)
                .ThenInclude(x => x.Office).Where(x => x.Appointment.Patient.ApplicationUserId == userId).ToListAsync();
            
            IEnumerable<int> ids = medicalrecords.Select(x => x.Appointment.Office.DoctorId);
            
            IEnumerable<DoctorDto> doctors = await GetAllDoctors(queryParameters);

            doctors = doctors.Where(x => ids.Contains(x.Id));

            return doctors.ToList();
        }

        public async Task<int> GetCountForAllDoctorsOfPatient(int userId, QueryParameters queryParameters)
        {
            var medicalrecords = await _context.MedicalRecords.Include(x => x.Appointment)
                .ThenInclude(x => x.Patient).Include(x => x.Appointment)
                .ThenInclude(x => x.Office).Where(x => x.Appointment.Patient.ApplicationUserId == userId).ToListAsync();
            
            IEnumerable<int> ids = medicalrecords.Select(x => x.Appointment.Office.DoctorId);
            
            IEnumerable<DoctorDto> doctors = await GetAllDoctors(queryParameters);

            doctors = doctors.Where(x => ids.Contains(x.Id));

            return doctors.Count();
        }

        public async Task<Doctor> FindDoctorById(int id)
        {
            return await _context.Doctors.Include(x => x.DoctorSpecialties).ThenInclude(x => x.Specialties)
                         .Include(x => x.DoctorHospitals).ThenInclude(x => x.Hospital)
                         .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Doctor> FindDoctorByUserId(int userId)
        {
            return await _context.Doctors.Include(x => x.DoctorSpecialties).ThenInclude(x => x.Specialties)
                         .Include(x => x.DoctorHospitals).ThenInclude(x => x.Hospital)
                         .Where(x => x.ApplicationUserId == userId).FirstOrDefaultAsync();
        }

        public async Task CreateDoctor(int userId, Doctor doctor, string lastname, string firstname)
        {
            doctor.ApplicationUserId = userId;
            doctor.Name = string.Format("{0} {1}", lastname, firstname);

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Office>> GetAllOfficesForDoctorById(int id)
        {
            return await _context.Offices.Where(x => x.DoctorId == id).ToListAsync();
        }

        public async Task<List<Office>> GetAllOfficesForDoctorByUserId(int userId)
        {
            return await _context.Offices.Include(x => x.Doctor)
                         .Where(x => x.Doctor.ApplicationUserId == userId).ToListAsync();
        }

        public async Task<List<Specialty>> GetNonSelectedSpecialties(List<int> ids)
        {
            return await _context.Specialties.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Hospital>> GetNonSelectedHospitals(List<int> ids)
        {
            return await _context.Hospitals.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }
        
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<DoctorStatisticsDto> ShowCountForEntitiesForDoctor(int userId)
        {
            var doctorstatistics = new DoctorStatisticsDto();

            var medicalrecords = await _context.MedicalRecords.Include(x => x.Appointment)
                .ThenInclude(x => x.Office).ThenInclude(x => x.Doctor)
                .Where(x => x.Appointment.Office.Doctor.ApplicationUserId == userId).ToListAsync();

            IEnumerable<int?> patientsids = medicalrecords.Select(x => x.Appointment.PatientId);

            doctorstatistics.AvailableAppointmentsCount = await _context. Appointments
                .Where(p => p.Status == null & p.PatientId == null && p.Office.Doctor.ApplicationUserId == userId
                && p.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync();
        
            doctorstatistics.UpcomingAppointmentsCount = await _context.Appointments
                .Where(x => x.Status == true && x.StartDateAndTimeOfAppointment > DateTime.Now 
                && x.Office.Doctor.ApplicationUserId == userId).CountAsync();

            doctorstatistics.MedicalRecordsCount = await _context.MedicalRecords
                .Where(x => x.Appointment.Office.Doctor.ApplicationUserId == userId).CountAsync();           

            doctorstatistics.PatientsCount = await _context.Patients
                .Where(x => patientsids.Contains(x.Id)).CountAsync();

            return doctorstatistics;
        }

        public async Task<IEnumerable<DoctorChartDto1>> GetNumberAndTypeOfAppointmentsForDoctorForChart(
                int userId)
        {
            List<DoctorChartDto1> list = new List<DoctorChartDto1>();
            
            list.Add(new DoctorChartDto1 { AppointmentType = "Upcoming", 
                NumberOfAppointments = await _context.Appointments
                .Where(p => p.Office.Doctor.ApplicationUserId == userId 
                && p.Status == true & p.PatientId != null
                && p.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync()  });

            list.Add(new DoctorChartDto1 { AppointmentType = "Available", 
                NumberOfAppointments = await _context.Appointments
                .Where(p => p.Office.Doctor.ApplicationUserId == userId 
                && p.Status == null & p.PatientId == null
                && p.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync()  });

            list.Add(new DoctorChartDto1 { AppointmentType = "Unconfirmed active", 
                NumberOfAppointments = await _context.Appointments
                .Where(p => p.Office.Doctor.ApplicationUserId == userId && 
                p.Status == null && p.PatientId != null
                && p.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync()  });

            list.Add(new DoctorChartDto1 { AppointmentType = "Previous attended", 
                NumberOfAppointments = await _context.Appointments
                .Where(p => p.Office.Doctor.ApplicationUserId == userId && 
                p.EndDateAndTimeOfAppointment < DateTime.Now && p.Status == true).CountAsync()  });

            list.Add(new DoctorChartDto1 { AppointmentType = "Previous non-attended", 
                NumberOfAppointments = await _context.Appointments
                .Where(p => p.Office.Doctor.ApplicationUserId == userId && 
                p.EndDateAndTimeOfAppointment < DateTime.Now && p.Status == null).CountAsync()  });
                                
            return list;
        }

        public async Task<IEnumerable<DoctorChartDto2>> GetNumberAndTypeOfPatientsForDoctorForChart(
                int userId)
        {
            List<DoctorChartDto2> list = new List<DoctorChartDto2>();

            var hospitalmedicalrecords = await _context.MedicalRecords.Include(x => x.Appointment)
                .ThenInclude(x => x.Office).ThenInclude(x => x.Doctor)
                .Where(x => x.Appointment.Office.Doctor.ApplicationUserId == userId 
                && x.Appointment.Office.HospitalId != null).ToListAsync();

            IEnumerable<int?> hospitalpatientsids = hospitalmedicalrecords.Select(x => x.Appointment.PatientId);

            var privatepracticemedicalrecords = await _context.MedicalRecords.Include(x => x.Appointment)
                .ThenInclude(x => x.Office).ThenInclude(x => x.Doctor)
                .Where(x => x.Appointment.Office.Doctor.ApplicationUserId == userId 
                && x.Appointment.Office.HospitalId == null).ToListAsync();

            IEnumerable<int?> privatepracticepatientsids = privatepracticemedicalrecords.Select(x => x.Appointment.PatientId);
          
            list.Add(new DoctorChartDto2 { PracticeType = "Hospital examination", 
                NumberOfPatients = await _context.Patients.Where(x => hospitalpatientsids.Contains(x.Id)).CountAsync()  });

            list.Add(new DoctorChartDto2 { PracticeType = "Private practice examination", 
                NumberOfPatients =  await _context.Patients.Where(x => privatepracticepatientsids.Contains(x.Id)).CountAsync()  });
                                
            return list;
        }

        public async Task<IEnumerable<DoctorChartDto3>> GetNumberAndTypeOfMedicalRecordsForDoctorForChart(
                int userId)
        {
            List<DoctorChartDto3> list = new List<DoctorChartDto3>();

            list.Add(new DoctorChartDto3 { MedicalRecordType = "Hospital medical records", 
                NumberOfMedicalRecords = await _context.MedicalRecords.Include(x => x.Appointment)
                .ThenInclude(x => x.Office).ThenInclude(x => x.Doctor)
                .Where(x => x.Appointment.Office.HospitalId != null &&
                x.Appointment.Office.Doctor.ApplicationUserId == userId).CountAsync()  });

            list.Add(new DoctorChartDto3 { MedicalRecordType = "Private practice medical records", 
                NumberOfMedicalRecords = await _context.MedicalRecords.Include(x => x.Appointment)
                .ThenInclude(x => x.Office).
                ThenInclude(x => x.Doctor).Where(x => x.Appointment.Office.HospitalId == null &&
                x.Appointment.Office.Doctor.ApplicationUserId == userId).CountAsync()  });
            
            return list;
        }


    }
}









