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
                                        Where(x => x.SpecialtyId == queryParameters.SpecialtyId) 
                                        .FirstOrDefaultAsync();

            IQueryable<Appointment> appointments = _context.Appointments.Include(x => x.Office)
                    .ThenInclude(x => x.Doctor)
                    .Where(x => x.PatientId == null && x.StartDateAndTimeOfAppointment > DateTime.Now)
                    .AsQueryable().OrderByDescending(x => x.StartDateAndTimeOfAppointment);
            
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
                    case "dateAsc":
                        appointments = appointments.OrderBy(p => p.StartDateAndTimeOfAppointment);
                        break;
                    default:
                        appointments = appointments.OrderByDescending(n => n.StartDateAndTimeOfAppointment);
                        break;
                }
            }           
            return await appointments.ToListAsync();        
        }

        public async Task<int> GetCountForAllAvailableUpcomingAppointments()
        {
            return await _context.Appointments
                         .Where(x => x.PatientId == null && x.StartDateAndTimeOfAppointment > DateTime.Now)
                         .CountAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsForSingleDoctor(QueryParameters queryParameters, int userId)
        {
            IQueryable<Appointment> appointment = _context.Appointments.Include(x => x.Patient)
                                                   .Include(x => x.Office).ThenInclude(x => x.Doctor)
                                                   .Where(x => x.Office.Doctor.ApplicationUserId == userId)
                                                   .AsQueryable().OrderBy(x => x.StartDateAndTimeOfAppointment);
            
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
                    case "pending":
                        appointment = appointment.Where(p => p.Status == null & p.PatientId == null);
                        break;
                    case "booked":
                        appointment = appointment.Where(p => p.Status == null && p.PatientId != null);
                        break;
                    case "confirmed":
                        appointment = appointment.Where(p => p.Status == true && p.PatientId != null);
                        break;
                    case "cancelled":
                        appointment = appointment.Where(p => p.Status == false && p.PatientId != null);
                        break;
                    default:
                        appointment = appointment
                            .Where(n => n.StartDateAndTimeOfAppointment > DateTime.Now && n.PatientId != null);
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
                     case "booked":
                        appointments = appointments.Where(p => p.Status == null && p.PatientId != null);
                        break;
                    case "confirmed":
                        appointments = appointments.Where(p => p.Status == true && p.PatientId != null);
                        break;
                    case "previous":
                        appointments = appointments.
                            Where(p => p.EndDateAndTimeOfAppointment < DateTime.Now && p.PatientId != null);
                        break;
                    case "dateDesc":
                        appointments = appointments.OrderByDescending(p => p.StartDateAndTimeOfAppointment);
                        break;
                    default:
                        appointments = appointments.OrderBy(n => n.StartDateAndTimeOfAppointment);
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
                                    .Include(x => x.Patient)
                                    .Include(x => x.Office).ThenInclude(x => x.Doctor)
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
                         && x.StartDateAndTimeOfAppointment > DateTime.Now)
                         .CountAsync();
        }

        public async Task<Appointment> GetApointmentById(int id)
        {
            return await _context.Appointments.Include(x => x.Office).ThenInclude(x => x.Doctor)
                         .Include(x => x.Patient).Where(x => x.Id == id)
                         .FirstOrDefaultAsync();        }

        public async Task CreateAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(Appointment appointment)
        {          
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
            return await _context.Offices.Where(x => x.Doctor.ApplicationUserId == userId)
                         .ToListAsync();
        }

    }
}













