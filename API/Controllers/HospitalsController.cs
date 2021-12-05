using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ErrorHandling;
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
    public class HospitalsController : BaseApiController
    {
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public HospitalsController(IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        [HttpGet("adminlist")]
        public async Task<ActionResult<Pagination<HospitalDto>>> GetAllHospitalsForAdminList(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _hospitalRepository.GetCountForAllHospitalsForAdminList();
            var list = await _hospitalRepository.GetAllHospitalsForAdminList(queryParameters);

            var data = _mapper.Map<IEnumerable<HospitalDto>>(list);

            return Ok(new Pagination<HospitalDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("office")]
        public async Task<ActionResult<List<HospitalDto>>> GetAllHospitalsForOffice()
        {
            var list = await _hospitalRepository.GetAllHospitalsForOffice();

            return _mapper.Map<List<HospitalDto>>(list);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalDto>> GetHospitalById(int id)
        {
            var hospital = await _hospitalRepository.GetHospitalById(id);

            if (hospital == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<HospitalDto>(hospital);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<ActionResult<HospitalDto>> CreateHospital([FromBody] HospitalDto hospitalDTO)
        {
            var hospital = _mapper.Map<Hospital>(hospitalDTO);

            await _hospitalRepository.CreateHospital(hospital);

            return CreatedAtAction("GetHospitalById", new {id = hospital.Id }, 
                _mapper.Map<HospitalDto>(hospital));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHospital(int id, [FromBody] HospitalDto hospitalDto)
        {
            var hospital = _mapper.Map<Hospital>(hospitalDto);

            if (id != hospital.Id) return BadRequest(new ServerResponse(400));

            await _hospitalRepository.UpdateHospital(hospital);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHospital(int id)
        {
            var hospital = await _hospitalRepository.GetHospitalById(id);

            if (hospital == null) return NotFound(new ServerResponse(404));

            await _hospitalRepository.DeleteHospital(hospital);

            return NoContent();
        }      
    }
}





