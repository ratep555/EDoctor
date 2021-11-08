using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SpecialtiesController : BaseApiController
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        public SpecialtiesController(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Specialty>> GetAllSpecialties()
        {
            var list = await _specialtyRepository.GetSpecialties();

            return Ok(list);
        }
    }
}








