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
    public class HospitalsController : BaseApiController
    {
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public HospitalsController(IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<HospitalDto>>> GetAllHospitals()
        {
            var list = await _hospitalRepository.GetAllHospitals();

            return _mapper.Map<List<HospitalDto>>(list);
        }

        
    }
}