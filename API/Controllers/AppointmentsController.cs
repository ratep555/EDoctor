using System.Collections.Generic;
using System.Threading.Tasks;
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
            var list = await _appointmentRepository
                             .GetAppointmentsForSinglePatient(userId, queryParameters);

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

        [HttpPost]
        public async Task<ActionResult> CreateAppointmentByDoctor([FromBody] AppointmentCreateEditDto appointmentDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentDto);
           
            await _appointmentRepository.CreateAppointment(appointment);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAppointmentByDoctor(int id, 
            [FromBody] AppointmentCreateEditDto appointmentDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentDto);

            if (id != appointment.Id) return BadRequest();
            
            appointment.StartDateAndTimeOfAppointment = appointmentDto.StartDateAndTimeOfAppointment.AddHours(1);
            appointment.EndDateAndTimeOfAppointment = appointmentDto.EndDateAndTimeOfAppointment.AddHours(1);

            await _appointmentRepository.UpdateAppointment(appointment);

            return NoContent();
        }

        [HttpPut("bookappointment/{id}")]
        public async Task<ActionResult> UpdateAppointmentByPatient(int id,
             [FromBody] AppointmentCreateEditDto appointmentDto)
        {
            var userId = User.GetUserId();

            var patient = await _patientRepository.FindPatientByUserId(userId);

            var appointment = await _appointmentRepository.GetApointmentById(id);

            if (appointment == null) return NotFound();
            
            appointment.PatientId = patient.Id;
            appointment.Remarks = appointmentDto.Remarks;

            await _appointmentRepository.UpdateAppointment(appointment);

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


