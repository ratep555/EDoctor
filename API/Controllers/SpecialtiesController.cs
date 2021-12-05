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
    public class SpecialtiesController : BaseApiController
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IMapper _mapper;

        public SpecialtiesController(ISpecialtyRepository specialtyRepository, IMapper mapper)
        {
            _specialtyRepository = specialtyRepository;
            _mapper = mapper;
        }

        [HttpGet("adminlist")]
        public async Task<ActionResult<Pagination<SpecialtyDto>>> GetAllSpecialtiesForAdminList(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _specialtyRepository.GetCountForAllSpecialtieForAdminList();
            var list = await _specialtyRepository.GetAllSpecialtiesForAdminList(queryParameters);

            var data = _mapper.Map<IEnumerable<SpecialtyDto>>(list);

            return Ok(new Pagination<SpecialtyDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet]
        public async Task<ActionResult<List<SpecialtyDto>>> GetAllSpecialties()
        {
            var list = await _specialtyRepository.GetAllSpecialties();

            return _mapper.Map<List<SpecialtyDto>>(list);
        }
        
        [HttpGet("attributedtodoctors")]
        public async Task<ActionResult<List<SpecialtyDto>>> GetSpecialtiesAttributetToDoctors()
        {
            var list = await _specialtyRepository.GetSpecialtiesAttributedToDoctors();

            return _mapper.Map<List<SpecialtyDto>>(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialtyDto>> GetSpecialtyById(int id)
        {
            var specialty = await _specialtyRepository.GetSpecialtyById(id);

            if (specialty == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<SpecialtyDto>(specialty);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<ActionResult<SpecialtyDto>> CreateSpecialty([FromBody] SpecialtyDto specialtyDto)
        {
            var specialty = _mapper.Map<Specialty>(specialtyDto);

            await _specialtyRepository.CreateSpecialty(specialty);

            return CreatedAtAction("GetSpecialtyById", new {id = specialty.Id }, 
                _mapper.Map<SpecialtyDto>(specialty));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSpecialty(int id, [FromBody] SpecialtyDto specialtyDto)
        {
            var specialty = _mapper.Map<Specialty>(specialtyDto);

            if (id != specialty.Id) return BadRequest(new ServerResponse(400));

            await _specialtyRepository.UpdateSpecialty(specialty);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSpecialty(int id)
        {
            var specialty = await _specialtyRepository.GetSpecialtyById(id);

            if (specialty == null) return NotFound(new ServerResponse(404));

            await _specialtyRepository.DeleteSpecialty(specialty);

            return NoContent();
        }
    }
}








