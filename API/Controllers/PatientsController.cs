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
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;
        public PatientsController(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
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
    }
}







