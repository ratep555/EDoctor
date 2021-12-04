using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
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
    public class OfficesController : BaseApiController
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private string containerName = "offices";

        public OfficesController(IOfficeRepository officeRepository,
            IDoctorRepository doctorRepository,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            _officeRepository = officeRepository;
            _doctorRepository = doctorRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        [HttpGet]
         public async Task<ActionResult<Pagination<OfficeDto>>> GetAllOffices(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _officeRepository.GetCountForAllOffices();
            var list = await _officeRepository.GetAllOffices(queryParameters);

            var data = _mapper.Map<IEnumerable<OfficeDto>>(list);

            return Ok(new Pagination<OfficeDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("singledoctor")]
        public async Task<ActionResult<Pagination<OfficeDto>>> GetAllOfficesForSingleDoctor(
            [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _officeRepository.GetCountForOfficesForDoctor(userId);
            var list = await _officeRepository.GetOfficesForDoctor(queryParameters, userId);

            var data = _mapper.Map<IEnumerable<OfficeDto>>(list);

            return Ok(new Pagination<OfficeDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeDto>> GetOfficeById(int id)
        {
            var office = await _officeRepository.GetOfficeById(id);

            if (office == null) return NotFound();

            return _mapper.Map<OfficeDto>(office);
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateOffice([FromForm] OfficeCreateEditDto officeDto)
        {
            var userId = User.GetUserId();

            var doctor = await _doctorRepository.FindDoctorByUserId(userId);

            var office = _mapper.Map<Office>(officeDto);

            office.DoctorId = doctor.Id;

            if (officeDto.Picture != null)
            {
                office.Picture = await _fileStorageService.SaveFile(containerName, officeDto.Picture);
            }

            await _officeRepository.CreateOffice(office);
           
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOffice(int id, [FromForm] OfficeCreateEditDto officeDto)
        {
            var userId = User.GetUserId();

            var doctor = await _officeRepository.FindDoctorByUserId(userId);

            var office = await _officeRepository.GetOfficeById(id);

            if (office == null) return NotFound();

            office = _mapper.Map(officeDto, office);
            
            office.DoctorId = doctor.Id;

            if (officeDto.Picture != null)
            {
                office.Picture = await _fileStorageService.EditFile(containerName, officeDto.Picture, office.Picture);
            }

            await _officeRepository.UpdateOffice(office);

            return NoContent();
        }

        [HttpGet("hospitals")]
        public async Task<ActionResult<List<HospitalDto>>> GetHospitalsForDoctor()
        {
            var userId = User.GetUserId();

            var list = await _officeRepository.GetHospitalsForDoctor(userId);

            return _mapper.Map<List<HospitalDto>>(list);
        }

    }
}





