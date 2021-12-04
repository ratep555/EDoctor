using System.Collections.Generic;
using System.Threading.Tasks;
using API.ErrorHandling;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class AppointmentsController : BaseApiController
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentRepository appointmentRepository, 
            IPatientRepository patientRepository, 
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<AppointmentDto>>> GetAllAvailableUpcomingAppointments(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _appointmentRepository.GetCountForAllAvailableUpcomingAppointments();
            var list = await _appointmentRepository
                .GetAllAvailableUpcomingAppointments(queryParameters);

            var data = _mapper.Map<IEnumerable<AppointmentDto>>(list);

            return Ok(new Pagination<AppointmentDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("singledoctor")]
         public async Task<ActionResult<Pagination<AppointmentDto>>> GetAllAppointmentsForSingleDoctor(
            [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _appointmentRepository.GetCountForAppointmentsForSingleDoctor(userId);
            var list = await _appointmentRepository
                .GetAppointmentsForSingleDoctor(queryParameters, userId);

            var data = _mapper.Map<IEnumerable<AppointmentDto>>(list);

            return Ok(new Pagination<AppointmentDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("singlepatient")]
         public async Task<ActionResult<Pagination<AppointmentDto>>> GetAppointmentsForSinglePatient(
            [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _appointmentRepository.GetCountForAppointmentsForSinglePatient(userId);
            var list = await _appointmentRepository.GetAppointmentsForSinglePatient(userId, queryParameters);

            var data = _mapper.Map<IEnumerable<AppointmentDto>>(list);

            return Ok(new Pagination<AppointmentDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("availableappointments/{id}")]
         public async Task<ActionResult<Pagination<AppointmentDto>>> GetAllAvailableAppointmentsForOffice(
            int id, [FromQuery] QueryParameters queryParameters)
        {
            var count = await _appointmentRepository.GetCountForAvailableAppointmentsForOffice(id);
            var list = await _appointmentRepository.GetAvailableAppointmentsForOffice(id, queryParameters);

            var data = _mapper.Map<IEnumerable<AppointmentDto>>(list);

            return Ok(new Pagination<AppointmentDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepository.GetApointmentById(id);

            if (appointment == null) return NotFound();

            return _mapper.Map<AppointmentDto>(appointment);
        }

        [Authorize(Policy = "RequireDoctorRole")]
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointmentByDoctor([FromBody] AppointmentCreateEditDto appointmentDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentDto);
           
            await _appointmentRepository.CreateAppointment(appointment);

            return CreatedAtAction("GetAppointmentById", new {id = appointment.Id }, _mapper.Map<AppointmentDto>(appointment));
        }

        [Authorize(Policy = "RequireDoctorRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAppointmentByDoctor(int id, 
            [FromBody] AppointmentCreateEditDto appointmentDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentDto);

            if (id != appointment.Id) return BadRequest();

            if (appointmentDto.Status == null) 
            {
                appointment.PatientId = null;
                appointment.Remarks = "";
            }

                await _appointmentRepository.UpdateAppointment(appointment);
                        
            return NoContent();
        }

        [Authorize(Policy = "RequirePatientRole")]
        [HttpPut("bookappointment/{id}")]
        public async Task<ActionResult> BookAppointmentByPatient(int id,
             [FromBody] AppointmentCreateEditDto appointmentDto)
        {
            var userId = User.GetUserId();

            var patient = await _patientRepository.FindPatientByUserId(userId);

            var appointment = _mapper.Map<Appointment>(appointmentDto);

            if (id != appointment.Id) return BadRequest();

            var office = await _appointmentRepository.GetOfficeByAppointment(appointment);

            if (patient.MBO == null && office.HospitalId != null )
            return BadRequest("You have not provided MBO in your registration form!");
            
            appointment.PatientId = patient.Id;

            await _appointmentRepository.UpdateAppointment(appointment);

            return NoContent();
        }

        [Authorize(Policy = "RequirePatientRole")]
        [HttpPut("cancelappointment/{id}")]
        public async Task<ActionResult> CancelAppointmentByPatient(int id)
        {
            var userId = User.GetUserId();
            
            var patient = await _patientRepository.FindPatientByUserId(userId);

            var appointment = await _appointmentRepository.GetApointmentById(id);

            if (appointment == null) return NotFound();

            if (appointment.Status == true) 
            {
                appointment.Status = null;
            }

            appointment.PatientId = null;
            appointment.Remarks = "";

            await _appointmentRepository.UpdateAppointment(appointment);

            return NoContent();
        }

        [Authorize(Policy = "RequireDoctorRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentRepository.GetApointmentById(id);

            if (appointment == null) return NotFound(new ServerResponse(404));

            await _appointmentRepository.DeleteAppointment(appointment);

            return NoContent();
        }      

        [HttpGet("offices")]
        public async Task<ActionResult<List<OfficeDto>>> GetDoctorsOffices()
        {
            var userId = User.GetUserId();

            var list = await _appointmentRepository.GetDoctorOffices(userId);
            return _mapper.Map<List<OfficeDto>>(list);
        }
        
    }
}


