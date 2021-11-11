using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SpecialtiesController : BaseApiController
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IMapper _mapper;

        public SpecialtiesController(ISpecialtyRepository specialtyRepository, IMapper mapper)
        {
            _specialtyRepository = specialtyRepository;
            _mapper = mapper;
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
    }
}








