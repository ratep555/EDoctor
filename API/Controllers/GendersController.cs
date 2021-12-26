using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class GendersController : BaseApiController
    {
        private readonly IGenderRepository _genderRepository;
        private readonly IMapper _mapper;

        public GendersController(IGenderRepository genderRepository, IMapper mapper)
        {
            _genderRepository = genderRepository;
            _mapper = mapper;
        }

        [HttpGet("adminlist")]
        public async Task<ActionResult<Pagination<GenderDto>>> GetAllGendersForAdminList(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _genderRepository.GetCountForAllGendersForAdminList();
            
            var list = await _genderRepository.GetAllGendersForAdminList(queryParameters);

            var data = _mapper.Map<IEnumerable<GenderDto>>(list);

            return Ok(new Pagination<GenderDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenderDto>> GetGenderById(int id)
        {
            var gender = await _genderRepository.GetGenderById(id);

            if (gender == null) return NotFound();

            return _mapper.Map<GenderDto>(gender);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<ActionResult<GenderDto>> CreateGender([FromBody] GenderDto genderDTO)
        {
            var gender = _mapper.Map<Gender>(genderDTO);

            await _genderRepository.CreateGender(gender);

            return CreatedAtAction("GetGenderById", new {id = gender.Id }, 
                _mapper.Map<GenderDto>(gender));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateGender(int id, [FromBody] GenderDto genderDto)
        {
            var gender = _mapper.Map<Gender>(genderDto);

            if (id != gender.Id) return BadRequest();

            await _genderRepository.UpdateGender(gender);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGender(int id)
        {
            var gender = await _genderRepository.GetGenderById(id);

            if (gender == null) return NotFound();

            await _genderRepository.DeleteGender(gender);

            return NoContent();
        }      
    }
}


