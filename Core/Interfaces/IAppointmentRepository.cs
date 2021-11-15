using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAvailableUpcomingAppointments(QueryParameters queryParameters);
        Task<int> GetCountForAllAvailableUpcomingAppointments();
        Task<List<Appointment>> GetAppointmentsForSingleDoctor(QueryParameters queryParameters, int userId);
        Task<int> GetCountForAppointmentsForSingleDoctor(int userId);
        Task<List<Appointment>> GetAppointmentsForSinglePatient(int userId, QueryParameters queryParameters);
        Task<int> GetCountForAppointmentsForSinglePatient(int userId);
        Task<List<Appointment>> GetAvailableAppointmentsForOffice(int id, QueryParameters queryParameters);
        Task<int> GetCountForAvailableAppointmentsForOffice(int id);
        Task<Appointment> GetApointmentById(int id);
        Task CreateAppointment(Appointment appointment);
        Task<List<Office>> GetDoctorOffices(int userId);
        Task UpdateAppointment(Appointment appointment);

    }
}