using System.Collections.Generic;
using System.Threading.Tasks;
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<PatientDto>>> GetAllPatientsOfDoctor(
                [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _patientRepository.GetCountForAllPatientsOfDoctor(userId);
            var list = await _patientRepository.GetAllPatientsOfDoctor(userId, queryParameters);

            var data = _mapper.Map<IEnumerable<PatientDto>>(list);

            return Ok(new Pagination<PatientDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));        
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(int id)
        {
            var patient = await _patientRepository.FindPatientById(id);

            if (patient == null) return NotFound();

            return _mapper.Map<PatientDto>(patient);
        }

        [HttpGet("userid/{id}")]
        public async Task<ActionResult<PatientDto>> GetPatientForMyProfile(int id)
        {
            var patient = await _patientRepository.FindPatientByUserId(id);

            if (patient == null) return NotFound();

            return _mapper.Map<PatientDto>(patient);
        }

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
    }
}







