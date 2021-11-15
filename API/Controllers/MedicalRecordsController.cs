using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MedicalRecordsController : BaseApiController
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        public MedicalRecordsController(IMedicalRecordRepository medicalRecordRepository,
            IPatientRepository patientRepository,
            IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        [HttpGet("doctorspatient/{id}")]
        public async Task<ActionResult<Pagination<MedicalRecordDto>>> GetMedicalRecordsForDoctorsPatient(
            int id, [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var patient = await _patientRepository.FindPatientById(id);

            if (patient == null) return NotFound();

            var count = await _medicalRecordRepository
                .GetCountForMedicalRecordsForDoctorsPatient(id, userId);

            var list = await _medicalRecordRepository
                .GetAllMedicalRecordsForDoctorsPatient(id, userId, queryParameters);

            var data = _mapper.Map<List<MedicalRecordDto>>(list);

            return Ok(new Pagination<MedicalRecordDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

     
    }
}









