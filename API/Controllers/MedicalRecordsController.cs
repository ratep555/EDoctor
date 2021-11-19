using System.Collections.Generic;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
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

        [HttpGet("allpatientsofdoctor")]
        public async Task<ActionResult<Pagination<MedicalRecordDto>>> GetMedicalRecordsForAllPatientsOfDoctor(
            [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _medicalRecordRepository.GetCountForMedicalRecordsForAllPatientsOfDoctor(userId);

            var list = await _medicalRecordRepository
                .GetMedicalRecordsForAllPatientsOfDoctor(userId, queryParameters);

            var data = _mapper.Map<List<MedicalRecordDto>>(list);

            return Ok(new Pagination<MedicalRecordDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("allrecordsforpatientasuser")]
        public async Task<ActionResult<Pagination<MedicalRecordDto>>> GetAllMedicalRecordsForPatientAsUser(
            [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _medicalRecordRepository.GetCountForAllMedicalRecordsForPatientAsUser(userId);

            var list = await _medicalRecordRepository.GetAllMedicalRecordsForPatientAsUser(userId, queryParameters);

            var data = _mapper.Map<List<MedicalRecordDto>>(list);

            return Ok(new Pagination<MedicalRecordDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

    

        [HttpGet("onepatientofdoctor/{id}")]
        public async Task<ActionResult<Pagination<MedicalRecordDto>>> GetMedicalRecordsForOnePatientOfDoctor(
            int id, [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var patient = await _patientRepository.FindPatientById(id);

            if (patient == null) return NotFound();

            var count = await _medicalRecordRepository
                .GetCountForMedicalRecordsForOnePatientOfDoctor(id, userId);

            var list = await _medicalRecordRepository
                .GetMedicalRecordsForOnePatientOfDoctor(id, userId, queryParameters);

            var data = _mapper.Map<List<MedicalRecordDto>>(list);

            return Ok(new Pagination<MedicalRecordDto>
            (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> CreateMedicalRecord(int id, 
            [FromBody] MedicalRecordCreateEditDto medicalRecordDto)
        {
            var medicalrecord = _mapper.Map<MedicalRecord>(medicalRecordDto);

            medicalrecord.PatientId = id;

            await _medicalRecordRepository.CreateMedicalRecord(medicalrecord);
           
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecordDto>> GetMedicalRecord(int id)
        {
            var record = await _medicalRecordRepository.FindMedicalRecordById(id);

            if (record == null) return NotFound();

            return _mapper.Map<MedicalRecordDto>(record);
            
        }

     
    }
}









