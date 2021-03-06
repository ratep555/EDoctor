using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ErrorHandling;
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
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private string container = "doctors";

        public DoctorsController(IDoctorRepository doctorRepository, 
            IRatingRepository ratingRepository,
            IAdminRepository adminRepository,
            IMapper mapper, 
            IFileStorageService fileStorageService)
        {
            _doctorRepository = doctorRepository;
            _ratingRepository = ratingRepository;
            _adminRepository = adminRepository;
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

        [HttpGet("doctorsofpatient")]
        public async Task<ActionResult<DoctorDto>> GetAllDoctorsOfPatient(
            [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _doctorRepository.GetCountForAllDoctorsOfPatient(userId, queryParameters);

            var list = await _doctorRepository.GetAllDoctorsOfPatient(userId, queryParameters);

            return Ok(new Pagination<DoctorDto>(queryParameters.Page, queryParameters.PageCount, count, list));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            var doctor = await _doctorRepository.FindDoctorById(id);

            if (doctor == null) return NotFound(new ServerResponse(404));

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

            if (doctor == null) return NotFound(new ServerResponse(404));

            var averageVote = 0.0;

            if (await _ratingRepository.ChechIfAnyForDoctorByUserId(id))
            {
                averageVote = await _ratingRepository.AverageVoteForDoctorByUserId(id);
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

            if (doctor == null) return NotFound(new ServerResponse(404));

            var doctorToReturn = _mapper.Map<DoctorDto>(doctor);

            var specialtiesSelectedIds = doctorToReturn.Specialties.Select(x => x.Id).ToList();

            var nonSelectedSpecialties = await _doctorRepository
                .GetNonSelectedSpecialties(specialtiesSelectedIds);

            var hospitalsSelectedIds = doctorToReturn.Hospitals.Select(x => x.Id).ToList();

            var nonSelectedHospitals = await _doctorRepository
                .GetNonSelectedHospitals(hospitalsSelectedIds);

            var nonSelectedSpecialtiesDto = _mapper.Map<IEnumerable<SpecialtyDto>>
                (nonSelectedSpecialties).OrderBy(x => x.SpecialtyName);

            var nonSelectedHospitalsDto = _mapper.Map<IEnumerable<HospitalDto>>
                (nonSelectedHospitals).OrderBy(x => x.HospitalName);

            var response = new DoctorPutGetDto();

            response.Doctor = doctorToReturn;
            response.SelectedSpecialties = doctorToReturn.Specialties.OrderBy(x => x.SpecialtyName);
            response.NonSelectedSpecialties = nonSelectedSpecialtiesDto;
            response.SelectedHospitals = doctorToReturn.Hospitals.OrderBy(x => x.HospitalName);
            response.NonSelectedHospitals = nonSelectedHospitalsDto;

            return response;
        }

        [HttpGet("doctorsofficesbyuserid")]
        public async Task<ActionResult<List<OfficeDto>>> GetAllOfficesForDoctorByUserId()
        {
            var userId = User.GetUserId();

            var list = await _doctorRepository.GetAllOfficesForDoctorByUserId(userId);

            return _mapper.Map<List<OfficeDto>>(list);
        }

        [Authorize(Policy = "RequireDoctorRole")]
        [HttpPut("updatingdoctorsprofile/{id}")]
        public async Task<ActionResult> UpdatingDoctorsProfile(int id, [FromForm] DoctorEditDto doctorDto)
        {
            var doctor = await _doctorRepository.FindDoctorById(id);

            if (doctor == null) return NotFound(new ServerResponse(404));

            doctor =  _mapper.Map(doctorDto, doctor);

            if (doctorDto.Picture != null)
            {
                doctor.Picture = await _fileStorageService.EditFile(container, doctorDto.Picture, doctor.Picture);
            }

            await _adminRepository.UpdateUserProfile(doctorDto);
 
            await _doctorRepository.Save();

            return NoContent();
        }    

        [HttpGet("doctorstatistics")]
        public async Task<ActionResult<StatisticsDto>> ShowCountForEntitiesForDoctor()
        {
            var userId = User.GetUserId();

            var list = await _doctorRepository.ShowCountForEntitiesForDoctor(userId);

            if (list == null) return NotFound(new ServerResponse(404));

            return Ok(list);
        }

        [HttpGet("doctorcharts1")]
        public async Task<ActionResult> ShowNumberAndTypeOfAppointmentsForDoctor()
        {
            var userId = User.GetUserId();

            var list = await _doctorRepository.GetNumberAndTypeOfAppointmentsForDoctorForChart(userId);

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest(new ServerResponse(400));        
        }

        [HttpGet("doctorcharts2")]
        public async Task<ActionResult> ShowNumberAndTypeOfPatientsForDoctor()
        {
            var userId = User.GetUserId();

            var list = await _doctorRepository.GetNumberAndTypeOfPatientsForDoctorForChart(userId);

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest(new ServerResponse(400));        
        }

        [HttpGet("doctorcharts3")]
        public async Task<ActionResult> ShowNumberAndTypeOfMedicalRecordsForDoctor()
        {
            var userId = User.GetUserId();

            var list = await _doctorRepository.GetNumberAndTypeOfMedicalRecordsForDoctorForChart(userId);

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest(new ServerResponse(400));        
        } 

        [HttpGet("doctorcharts4")]
        public async Task<ActionResult> ShowNumberAndTypeOfPatientsGenderForDoctor()
        {
            var userId = User.GetUserId();

            var list = await _doctorRepository.GetNumberAndTypeOfPatientsGenderForDoctorForChart(userId);

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest(new ServerResponse(400));        
        }     
    }
}



