using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ErrorHandling;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Interfaces;
using Core.Utilities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PatientsController : BaseApiController
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        public PatientsController(IPatientRepository patientRepository,
            IAdminRepository adminRepository,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<PatientDto>>> GetAllPatientsOfDoctor(
                [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _patientRepository.GetCountForAllPatientsOfDoctor(userId);
            
            var list = await _patientRepository.GetAllPatientsOfDoctor(userId, queryParameters);

            var data = _mapper.Map<IEnumerable<PatientDto>>(list);

            return Ok(new Pagination<PatientDto>(queryParameters.Page, queryParameters.PageCount, count, data));        
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(int id)
        {
            var patient = await _patientRepository.FindPatientById(id);

            if (patient == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<PatientDto>(patient);
        }

        [HttpGet("userid/{id}")]
        public async Task<ActionResult<PatientDto>> GetPatientForMyProfile(int id)
        {
            var patient = await _patientRepository.FindPatientByUserId(id);

            if (patient == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<PatientDto>(patient);
        }

        [AllowAnonymous]
        [HttpGet("genders")]
        public async Task<ActionResult<IEnumerable<GenderDto>>> GetGenders()
        {
            var list = await _patientRepository.GetGendersForPatient();

            var genders = _mapper.Map<IEnumerable<GenderDto>>(list);

            return Ok(genders);        
        }

        [Authorize(Policy = "RequirePatientRole")]
        [HttpPut("updatingpatientsprofile/{id}")]
        public async Task<ActionResult> UpdatingPatientsProfile(int id, [FromBody] PatientEditDto patientDto)
        {
            var patient = await _patientRepository.FindPatientById(id);

            if (patient == null) return NotFound();

            patient =  _mapper.Map(patientDto, patient);

            await _adminRepository.UpdateUserPatientProfile(patientDto);
 
            await _patientRepository.Save();

            return NoContent();
        }

        [HttpGet("patientstatistics")]
        public async Task<ActionResult<StatisticsDto>> ShowCountForEntitiesForPatient()
        {
            var userId = User.GetUserId();

            var list = await _patientRepository.ShowCountForEntitiesForPatient(userId);

            if (list == null) return NotFound(new ServerResponse(404));

            return Ok(list);
        }

        [HttpGet("patientcharts1")]
        public async Task<ActionResult> ShowNumberAndTypeOfMedicalRecordsForChartForPatient()
        {
            var userId = User.GetUserId();

            var list = await _patientRepository.GetNumberAndTypeOfMedicalRecordsForPatientForChart(userId);

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest(new ServerResponse(400));        
        }

        [HttpGet("patientcharts2")]
        public async Task<ActionResult> ShowNumberAndTypeOfOfficesForPatient()
        {
            var userId = User.GetUserId();

            var list = await _patientRepository.GetNumberAndTypeOfOfficesForPatientForChart(userId);

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest(new ServerResponse(400));        
        }

        [HttpGet("patientcharts3")]
        public async Task<ActionResult> ShowNumberAndTypeOfAppointmentsForPatient()
        {
            var userId = User.GetUserId();

            var list = await _patientRepository.GetNumberAndTypeOfAppointmentsForPatientForChart(userId);

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest(new ServerResponse(400));        
        }
    }
}







