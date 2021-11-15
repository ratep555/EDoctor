using System.Collections.Generic;
using System.Linq;
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
    [Authorize]
    public class DoctorsController : BaseApiController
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private string container = "doctors";

        public DoctorsController(IDoctorRepository doctorRepository, 
            IRatingRepository ratingRepository,
            IMapper mapper, 
            IFileStorageService fileStorageService)
        {
            _doctorRepository = doctorRepository;
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<DoctorDto>>> GetAllDoctors(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _doctorRepository.GetCountForAllDoctors();

            var list = await _doctorRepository.GetAllDoctors(queryParameters);

            return Ok(new Pagination<DoctorDto>
            (queryParameters.Page, queryParameters.PageCount, count, list));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            var doctor = await _doctorRepository.FindDoctorById(id);

            if (doctor == null) return NotFound();

            var averageVote = 0.0;
            var userVote = 0;

            if (await _ratingRepository.ChechIfAny(id))
            {
                averageVote = await _ratingRepository.AverageVote(id);
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();

                var ratingDb = await _ratingRepository.FindCurrentRate(id, userId);

                if (ratingDb != null)
                {
                    userVote = ratingDb.Rate;
                }
            }

            var offices = await _doctorRepository.GetAllOfficesForDoctorById(id);
            var officesDto = _mapper.Map<List<OfficeDto>>(offices); 

            var doctorToReturn = _mapper.Map<DoctorDto>(doctor);

            doctorToReturn.AverageVote = averageVote;
            doctorToReturn.UserVote = userVote;
            doctorToReturn.Offices = officesDto;

            return Ok(doctorToReturn);
        }

        [HttpGet("userid/{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctorForMyProfile(int id)
        {
            var doctor = await _doctorRepository.FindDoctorByUserId(id);

            if (doctor == null) return NotFound();

            var averageVote = 0.0;

            if (await _ratingRepository.ChechIfAny(id))
            {
                averageVote = await _ratingRepository.AverageVote(id);
            }

            var offices = await _doctorRepository.GetAllOfficesForDoctorByUserId(id);
            var officesDto = _mapper.Map<List<OfficeDto>>(offices); 

            var doctorToReturn = _mapper.Map<DoctorDto>(doctor);

            doctorToReturn.AverageVote = averageVote;
            doctorToReturn.Offices = officesDto;

            return Ok(doctorToReturn);
        }

        [HttpGet("putget/{id}")]
        public async Task<ActionResult<DoctorPutGetDto>> GetDoctorByIdForEditing(int id)
        {
            var doctor = await _doctorRepository.FindDoctorById(id);

            if (doctor == null) return NotFound();

            var doctorToReturn = _mapper.Map<DoctorDto>(doctor);

            var specialtiesSelectedIds = doctorToReturn.Specialties.Select(x => x.Id).ToList();

            var nonSelectedSpecialties = await _doctorRepository
                .GetNonSelectedSpecialties(specialtiesSelectedIds);

            var hospitalsSelectedIds = doctorToReturn.Hospitals.Select(x => x.Id).ToList();

            var nonSelectedHospitals = await _doctorRepository
                .GetNonSelectedHospitals(hospitalsSelectedIds);

            var nonSelectedSpecialtiesDto = _mapper.Map<List<SpecialtyDto>>(nonSelectedSpecialties);

            var nonSelectedHospitalsDto = _mapper.Map<List<HospitalDto>>(nonSelectedHospitals);

            var response = new DoctorPutGetDto();

            response.Doctor = doctorToReturn;
            response.SelectedSpecialties = doctorToReturn.Specialties;
            response.NonSelectedSpecialties = nonSelectedSpecialtiesDto;
            response.SelectedHospitals = doctorToReturn.Hospitals;
            response.NonSelectedHospitals = nonSelectedHospitalsDto;

            return response;
        }

        [HttpPut("updatingdoctorsprofile/{id}")]
        public async Task<ActionResult> UpdatingDoctorsProfile(int id, [FromForm] DoctorEditDto doctorDto)
        {
            var doctor = await _doctorRepository.FindDoctorById(id);

            if (doctor == null) return NotFound();

            doctor =  _mapper.Map(doctorDto, doctor);

            if (doctorDto.Picture != null)
            {
                doctor.Picture = await _fileStorageService.EditFile(container, doctorDto.Picture, doctor.Picture);
            }
 
            await _doctorRepository.Save();

            return NoContent();
        }




    }
}