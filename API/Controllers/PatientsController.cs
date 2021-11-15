using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Interfaces;
using Infrastructure.Data;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(int id)
        {
            var patient = await _patientRepository.FindPatientById(id);

            if (patient == null) return NotFound();

            return _mapper.Map<PatientDto>(patient);
        }
    }
}







