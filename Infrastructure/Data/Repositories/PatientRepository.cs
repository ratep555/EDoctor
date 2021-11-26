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
    public class PatientRepository : IPatientRepository
    {
        private readonly EDoctorContext _context;

        public PatientRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllPatientsOfDoctor(int userId,
            QueryParameters queryParameters)
        {   
            var medicalrecords = await _context.MedicalRecords.Include(x => x.Office).ThenInclude(x => x.Doctor)
                                 .Where(x => x.Office.Doctor.ApplicationUserId == userId).ToListAsync();

            IEnumerable<int> ids = medicalrecords.Select(x => x.PatientId);

            IQueryable<Patient> patients =  _context.Patients.Include(x => x.ApplicationUser)
                                            .Where(x => ids.Contains(x.Id))
                                            .AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                patients = patients
                .Where(x => x.Name.Contains(queryParameters.Query));
            }

            patients = patients.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                               .Take(queryParameters.PageCount);
            
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "nameDesc":
                        patients = patients.OrderByDescending(p => p.Name);
                        break;
                    default:
                        patients = patients.OrderBy(n => n.Name);
                        break;
                }
            }     
            return await patients.ToListAsync();        
        }

        public async Task<int> GetCountForAllPatientsOfDoctor(int userId)
        {
            var medicalrecords = await _context.MedicalRecords.Include(x => x.Office).ThenInclude(x => x.Doctor)
                                 .Where(x => x.Office.Doctor.ApplicationUserId == userId).ToListAsync();

            IEnumerable<int> ids = medicalrecords.Select(x => x.PatientId);

            return await _context.Patients.Where(x => ids.Contains(x.Id)).CountAsync();
        }

        public async Task<Patient> FindPatientByUserId(int userId)
        {
            return await _context.Patients.Include(x => x.ApplicationUser)
                         .Where(x => x.ApplicationUserId == userId)
                         .FirstOrDefaultAsync();
        }

        public async Task<Patient> FindPatientById(int id)
        {
            return await _context.Patients.Include(x => x.ApplicationUser)
                         .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreatePatient(ApplicationUser user, RegisterDto registerDto)
        {
            var patient = new Patient();
            patient.ApplicationUserId = user.Id;
            patient.Name = string.Format("{0} {1}", user.FirstName, user.LastName);
            patient.DateOfBirth = registerDto.DateOfBirth;
            patient.Street = registerDto.Street;
            patient.City = registerDto.City;
            patient.Country = registerDto.Country;
            patient.MBO = registerDto.MBO;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PatientStatisticsDto> ShowCountForEntitiesForPatient(int userId)
        {
            var patientstatistics = new PatientStatisticsDto();

            var medicalrecords = await _context.MedicalRecords.Include(x => x.Patient)
                .Include(x => x.Office).Where(x => x.Patient.ApplicationUserId == userId).ToListAsync();
            
            IEnumerable<int> doctorids = medicalrecords.Select(x => x.Office.DoctorId);
            IEnumerable<int> officeids = medicalrecords.Select(x => x.OfficeId);

            patientstatistics.AppointmentsCount = await _context.Appointments.Include(x => x.Patient)
                .Where(x => x.Patient.ApplicationUserId == userId && x.Status == true 
                && x.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync();
        
            patientstatistics.DoctorsCount = await _context.Doctors
                .Where(x => doctorids.Contains(x.Id)).CountAsync();

            patientstatistics.MedicalRecordsCount = await _context.MedicalRecords.Include(x => x.Patient)
                .Where(x => x.Patient.ApplicationUserId == userId).CountAsync();

            patientstatistics.OfficesCount = await _context.Offices
                .Where(x => officeids.Contains(x.Id)).CountAsync();

            return patientstatistics;
        }

        public async Task<IEnumerable<PatientChartDto1>> GetNumberAndTypeOfMedicalRecordsForPatientForChart(int userId)
        {
            List<PatientChartDto1> list = new List<PatientChartDto1>();

            list.Add(new PatientChartDto1 { MedicalRecordType = "Hospital medical records", 
                NumberOfMedicalRecords = await _context.MedicalRecords.Include(x => x.Patient)
                .Include(x => x.Office).Where(x => x.Office.HospitalId != null &&
                x.Patient.ApplicationUserId == userId).CountAsync()  });

            list.Add(new PatientChartDto1 { MedicalRecordType = "Private practice medical records", 
                NumberOfMedicalRecords = await _context.MedicalRecords.Include(x => x.Patient)
                .Include(x => x.Office).Where(x => x.Office.HospitalId == null &&
                x.Patient.ApplicationUserId == userId).CountAsync()  });
            
            return list;
        }

        public async Task<IEnumerable<PatientChartDto2>> GetNumberAndTypeOfOfficesForPatientForChart(int userId)
        {
            var medicalrecords = await _context.MedicalRecords.Include(x => x.Patient)
                .Include(x => x.Office).Where(x => x.Patient.ApplicationUserId == userId).ToListAsync();
            
            IEnumerable<int> hospitalofficesids = medicalrecords
                .Where(x => x.Office.HospitalId != null).Select(x => x.OfficeId);

            IEnumerable<int> nonhospitalofficesids = medicalrecords
                .Where(x => x.Office.HospitalId == null).Select(x => x.OfficeId);

            List<PatientChartDto2> list = new List<PatientChartDto2>();

            list.Add(new PatientChartDto2 { OfficeType = "Hospital offices", 
                NumberOfOffices = await _context.Offices
                .Where(p => hospitalofficesids.Contains(p.Id) 
                && !nonhospitalofficesids.Contains(p.Id)).CountAsync()  });

            list.Add(new PatientChartDto2 { OfficeType = "Private offices", 
                NumberOfOffices = await _context.Offices.Where(p => !hospitalofficesids.Contains(p.Id) 
                && nonhospitalofficesids.Contains(p.Id)).CountAsync()  });
                                
            return list;
        }

        public async Task<IEnumerable<PatientChartDto3>> GetNumberAndTypeOfAppointmentsForPatientForChart(
                int userId)
        {
            List<PatientChartDto3> list = new List<PatientChartDto3>();

            list.Add(new PatientChartDto3 { AppointmentType = "Upcoming", 
                NumberOfAppointments = await _context.Appointments
                .Where(p => p.Patient.ApplicationUserId == userId && p.Status == true
                && p.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync()  });

            list.Add(new PatientChartDto3 { AppointmentType = "Pending", 
                NumberOfAppointments = await _context.Appointments
                .Where(p => p.Patient.ApplicationUserId == userId && p.Status == null
                && p.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync()  });

            list.Add(new PatientChartDto3 { AppointmentType = "Previous", 
                NumberOfAppointments = await _context.Appointments
                .Where(p => p.Patient.ApplicationUserId == userId && p.EndDateAndTimeOfAppointment < DateTime.Now)
                .CountAsync()  });
                                
            return list;
        }
    }
}









