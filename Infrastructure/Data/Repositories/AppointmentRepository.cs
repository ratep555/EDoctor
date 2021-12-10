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
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly EDoctorContext _context;

        public AppointmentRepository(EDoctorContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAvailableUpcomingAppointments(QueryParameters queryParameters)
        {                                 
            var doctorSpecialty = await _context.DoctorSpecialties.
                Where(x => x.SpecialtyId == queryParameters.SpecialtyId).FirstOrDefaultAsync();

            IQueryable<Appointment> appointments = _context.Appointments.Include(x => x.Office)
                .ThenInclude(x => x.Doctor)
                .Include(x => x.Office).ThenInclude(x => x.Hospitals)
                .Where(x => x.PatientId == null && x.StartDateAndTimeOfAppointment > DateTime.Now)
                .AsQueryable();
            
            if (queryParameters.HasQuery())
            {
                appointments = appointments
                    .Where(x => x.Office.City.Contains(queryParameters.Query));
            }

            if (queryParameters.SpecialtyId.HasValue)
            {
                appointments = appointments.Where(x => x.Office.DoctorId == doctorSpecialty.DoctorId);
            }

            appointments = appointments.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "dateDesc":
                        appointments = appointments.OrderByDescending(p => p.StartDateAndTimeOfAppointment);
                        break;
                    case "hospital":
                        appointments = appointments.Where(p => p.Office.HospitalId != null);
                        break;
                    case "private":
                        appointments = appointments.Where(p => p.Office.HospitalId == null);
                        break;
                    default:
                        appointments = appointments.OrderBy(n => n.StartDateAndTimeOfAppointment);
                        break;
                }
            }           
            return await appointments.ToListAsync();        
        }

        public async Task<int> GetCountForAllAvailableUpcomingAppointments()
        {
            return await _context.Appointments
                .Where(x => x.PatientId == null && x.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsForSingleDoctor(QueryParameters queryParameters, int userId)
        {
            IQueryable<Appointment> appointment = _context.Appointments.Include(x => x.Patient)
                .Include(x => x.MedicalRecord).Include(x => x.Office)
                .ThenInclude(x => x.Doctor).Include(x => x.Office).ThenInclude(x => x.Hospitals)
                .Where(x => x.Office.Doctor.ApplicationUserId == userId).AsQueryable().OrderBy(x => x.StartDateAndTimeOfAppointment);
            
            if (queryParameters.HasQuery())
            {
                appointment = appointment
                    .Where(x => x.Office.Street.Contains(queryParameters.Query)
                    || x.Patient.Name.Contains(queryParameters.Query));
            }

            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "all":
                        appointment = appointment.OrderBy(p => p.StartDateAndTimeOfAppointment);
                        break;
                    case "active":
                        appointment = appointment.Where(p => p.StartDateAndTimeOfAppointment > DateTime.Now);
                        break;
                    case "upcoming":
                        appointment = appointment.Where(p => p.Status == true & p.PatientId != null
                        && p.StartDateAndTimeOfAppointment > DateTime.Now);
                        break;
                    case "available":
                        appointment = appointment.Where(p => p.Status == null & p.PatientId == null
                        && p.StartDateAndTimeOfAppointment > DateTime.Now);
                        break;
                    case "unconfirmed":
                        appointment = appointment.Where(p => p.Status == null && p.PatientId != null
                        && p.StartDateAndTimeOfAppointment > DateTime.Now);
                        break;
                    case "previousattended":
                        appointment = appointment.Where(x => x.EndDateAndTimeOfAppointment < DateTime.Now
                        && x.Status == true);
                        break;
                    case "previousnonattended":
                        appointment = appointment.Where(x => x.EndDateAndTimeOfAppointment < DateTime.Now
                        && x.Status == null);
                        break;
                    default:
                        appointment = appointment
                           .Where(p => p.StartDateAndTimeOfAppointment > DateTime.Now);
                        break;
                }
            }    

            appointment = appointment.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
            
            return await appointment.ToListAsync();        
        }

        public async Task<int> GetCountForAppointmentsForSingleDoctor(int userId)
        {
            return await _context.Appointments.Include(x => x.Office).ThenInclude(x => x.Doctor)
                .Where(x => x.Office.Doctor.ApplicationUserId == userId).CountAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsForSinglePatient(int userId,
            QueryParameters queryParameters)
        {
            IQueryable<Appointment> appointments = _context.Appointments.Include(x => x.Patient)
                .Include(x => x.Office).ThenInclude(x => x.Doctor)
                .Where(x => x.Patient.ApplicationUserId == userId)
                .AsQueryable().OrderBy(x => x.StartDateAndTimeOfAppointment);
            
            if (queryParameters.HasQuery())
            {
                appointments = appointments
                .Where(x => x.Office.Doctor.Name.Contains(queryParameters.Query));
            }

            appointments = appointments.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
            
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "all":
                        appointments = appointments.OrderBy(n => n.StartDateAndTimeOfAppointment);
                        break;
                    case "pending":
                        appointments = appointments.Where(p => p.Status == null
                        && p.StartDateAndTimeOfAppointment > DateTime.Now);
                        break;
                    case "previous":
                        appointments = appointments.
                        Where(p => p.EndDateAndTimeOfAppointment < DateTime.Now);
                        break;
                    case "dateDesc":
                        appointments = appointments.OrderByDescending(p => p.StartDateAndTimeOfAppointment);
                        break;
                    default:
                        appointments = appointments.Where(p => p.Status == true
                        && p.StartDateAndTimeOfAppointment > DateTime.Now);
                        break;                     
                }
            }               
            return await appointments.ToListAsync();        
        }

        public async Task<int> GetCountForAppointmentsForSinglePatient(int userId)
        {
            return await _context.Appointments.Include(x => x.Patient)
                .Where(x => x.Patient.ApplicationUserId == userId).CountAsync();
        }

        public async Task<List<Appointment>> GetAvailableAppointmentsForOffice(
            int id, QueryParameters queryParameters)
        {
            IQueryable<Appointment> appointment = _context.Appointments
                .Where(x => x.OfficeId == id && x.PatientId == null 
                && x.StartDateAndTimeOfAppointment > DateTime.Now)
                .Include(x => x.Patient).Include(x => x.Office).ThenInclude(x => x.Doctor)
                .AsQueryable().OrderBy(x => x.StartDateAndTimeOfAppointment);
            
            if (queryParameters.HasQuery())
            {
                appointment = appointment
                    .Where(x => x.StartDateAndTimeOfAppointment.ToString().Contains(queryParameters.Query));
            }

            appointment = appointment.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
            
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "dateDesc":
                        appointment = appointment.OrderByDescending(p => p.StartDateAndTimeOfAppointment);
                        break;
                    default:
                        appointment = appointment.OrderBy(n => n.StartDateAndTimeOfAppointment);
                        break;
                }
            }               
            return await appointment.ToListAsync();        
        }

        public async Task<int> GetCountForAvailableAppointmentsForOffice(int id)
        {
            return await _context.Appointments.Where(x => x.OfficeId == id && x.PatientId == null
                && x.StartDateAndTimeOfAppointment > DateTime.Now).CountAsync();
        }

        public async Task<Appointment> GetApointmentById(int id)
        {
            return await _context.Appointments.Include(x => x.Office).ThenInclude(x => x.Doctor)
                .Include(x => x.MedicalRecord).Include(x => x.Patient).Where(x => x.Id == id).FirstOrDefaultAsync();        }

        public async Task CreateAppointment(Appointment appointment)
        {
            appointment.StartDateAndTimeOfAppointment = appointment.StartDateAndTimeOfAppointment.ToLocalTime();
            appointment.EndDateAndTimeOfAppointment = appointment.EndDateAndTimeOfAppointment.ToLocalTime();
            
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(Appointment appointment)
        {       
            _context.ChangeTracker.Clear();
            
            appointment.StartDateAndTimeOfAppointment = appointment.StartDateAndTimeOfAppointment.ToLocalTime();
            appointment.EndDateAndTimeOfAppointment = appointment.EndDateAndTimeOfAppointment.ToLocalTime();
            
             _context.Entry(appointment).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointment(Appointment apponitment)
        {
            _context.Appointments.Remove(apponitment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Office>> GetDoctorOffices(int userId)
        {
            return await _context.Offices.Where(x => x.Doctor.ApplicationUserId == userId).ToListAsync();
        }

        public async Task<Office> GetOfficeByAppointment(Appointment appointment)
        {
            var list = await _context.Appointments.Where(x => x.Id == appointment.Id).ToListAsync();

            IEnumerable<int> ids = list.Select(x => x.OfficeId);

            return await _context.Offices.Where(x => ids.Contains(x.Id)).FirstOrDefaultAsync();
        }
    }
}













