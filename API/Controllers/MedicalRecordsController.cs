using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.ErrorHandling;
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

            if (patient == null) return NotFound(new ServerResponse(404));

            var count = await _medicalRecordRepository
                .GetCountForMedicalRecordsForOnePatientOfDoctor(id, userId);

            var list = await _medicalRecordRepository
                .GetMedicalRecordsForOnePatientOfDoctor(id, userId, queryParameters);

            var data = _mapper.Map<List<MedicalRecordDto>>(list);

            return Ok(new Pagination<MedicalRecordDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecordDto>> GetMedicalRecordById(int id)
        {
            var record = await _medicalRecordRepository.FindMedicalRecordById(id);

            if (record == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<MedicalRecordDto>(record);         
        }

        [Authorize(Policy = "RequireDoctorRole")]
        [HttpPost("{id}")]
        public async Task<ActionResult<MedicalRecordDto>> CreateMedicalRecord(int id, 
            [FromBody] MedicalRecordCreateEditDto medicalRecordDto)
        {
            var medicalrecord = _mapper.Map<MedicalRecord>(medicalRecordDto);

            medicalrecord.AppointmentId = id;

            await _medicalRecordRepository.CreateMedicalRecord(medicalrecord);
           
            return CreatedAtAction("GetMedicalRecordById", new {id = medicalrecord.AppointmentId }, 
                _mapper.Map<MedicalRecordDto>(medicalrecord));
        }    

        [Authorize(Policy = "RequireDoctorRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMedicalRecord(int id, 
            [FromBody] MedicalRecordCreateEditDto medicalRecordDto)
        {
            var medicalrecord = _mapper.Map<MedicalRecord>(medicalRecordDto);

            if (id != medicalrecord.AppointmentId) return BadRequest(new ServerResponse(400));

            await _medicalRecordRepository.UpdateMedicalRecord(medicalrecord);
           
            return NoContent();
        }
    }
}









