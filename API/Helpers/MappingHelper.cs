using AutoMapper;
using Core.Entities;
using Core.Dtos;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace API.Helpers
{
    public class MappingHelper : Profile
    {
        public MappingHelper(GeometryFactory geometryFactory)
        {
            CreateMap<Appointment, AppointmentDto>()
            .ForMember(d => d.OfficeAddress, o => o.MapFrom(s => s.Office.Street))
            .ForMember(d => d.AppointmentId, o => o.MapFrom(s => s.MedicalRecord.AppointmentId))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Office.City))
            .ForMember(d => d.Country, o => o.MapFrom(s => s.Office.Country))
            .ForMember(d => d.Patient, o => o.MapFrom(s => s.Patient.Name))
            .ForMember(d => d.Doctor, o => o.MapFrom(s => s.Office.Doctor.Name))
            .ForMember(d => d.Hospital, o => o.MapFrom(s => s.Office.Hospitals.HospitalName))
            .ForMember(d => d.DoctorId, o => o.MapFrom(s => s.Office.DoctorId));

            CreateMap<AppointmentCreateEditDto, Appointment>().ReverseMap();

            CreateMap<RegisterDto, ApplicationUser>().ReverseMap();

            CreateMap<DoctorCreateDto, ApplicationUser>().ReverseMap();

            CreateMap<Doctor, DoctorDto>()
                .ForMember(d => d.Specialties, o => o.MapFrom(MapForSpecialties2))
                .ForMember(d => d.Hospitals, o => o.MapFrom(MapForHospitals2));
            
            CreateMap<DoctorCreateDto, Doctor>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.DoctorSpecialties, options => options.MapFrom(MapDoctorSpecialties))
                .ForMember(x => x.DoctorHospitals, options => options.MapFrom(MapDoctorHospitals));

            CreateMap<DoctorEditDto, Doctor>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.DoctorSpecialties, options => options.MapFrom(MapDoctorSpecialties1))
                .ForMember(x => x.DoctorHospitals, options => options.MapFrom(MapDoctorHospitals1));
           
            CreateMap<Hospital, HospitalDto>().ReverseMap();

            CreateMap<MedicalRecord, MedicalRecordDto>()
                .ForMember(d => d.AppointmentId, o => o.MapFrom(s => s.AppointmentId))
                .ForMember(d => d.Doctor, o => o.MapFrom(s => s.Appointment.Office.Doctor.Name))
                .ForMember(d => d.DoctorId, o => o.MapFrom(s => s.Appointment.Office.Doctor.Id))
                .ForMember(d => d.Hospital, o => o.MapFrom(s => s.Appointment.Office.Hospitals.HospitalName))
                .ForMember(d => d.Office, o => o.MapFrom(s => s.Appointment.Office.Street))
                .ForMember(d => d.Patient, o => o.MapFrom(s => s.Appointment.Patient.Name))
                .ForMember(d => d.PatientId, o => o.MapFrom(s => s.Appointment.Patient.Id));

            CreateMap<MedicalRecordCreateEditDto, MedicalRecord>().ReverseMap();

            CreateMap<Office, OfficeDto>()
                .ForMember(d => d.Doctor, o => o.MapFrom(s => s.Doctor.Name))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Location.Y))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Location.X));

            CreateMap<OfficeCreateEditDto, Office>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            CreateMap<Patient, PatientDto>()
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.ApplicationUser.PhoneNumber))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.ApplicationUser.Email));
            
            CreateMap<PatientEditDto, Patient>();

            CreateMap<Specialty, SpecialtyDto>().ReverseMap();     
        }

        private List<DoctorSpecialty> MapDoctorSpecialties(DoctorCreateDto doctorDto, Doctor doctor)
        {
            var result = new List<DoctorSpecialty>();

            if (doctorDto.SpecialtiesIds == null) { return result; }

            foreach (var id in doctorDto.SpecialtiesIds)
            {
                result.Add(new DoctorSpecialty() { SpecialtyId = id });
            }
            return result;
        }

        private List<DoctorHospital> MapDoctorHospitals(DoctorCreateDto doctorDto, Doctor doctor)
        {
            var result = new List<DoctorHospital>();

            if (doctorDto.HospitalsIds == null) { return result; }

            foreach (var id in doctorDto.HospitalsIds)
            {
                result.Add(new DoctorHospital() { HospitalId = id });
            }
            return result;
        }

        private List<DoctorSpecialty> MapDoctorSpecialties1(DoctorEditDto doctorDto, Doctor doctor)
        {
            var result = new List<DoctorSpecialty>();

            if (doctorDto.SpecialtiesIds == null) { return result; }

            foreach (var id in doctorDto.SpecialtiesIds)
            {
                result.Add(new DoctorSpecialty() { SpecialtyId = id });
            }
            return result;
        }

        private List<DoctorHospital> MapDoctorHospitals1(DoctorEditDto doctorDto, Doctor doctor)
        {
            var result = new List<DoctorHospital>();

            if (doctorDto.HospitalsIds == null) { return result; }

            foreach (var id in doctorDto.HospitalsIds)
            {
                result.Add(new DoctorHospital() { HospitalId = id });
            }
            return result;
        }

        private List<SpecialtyDto> MapForSpecialties2(Doctor doctor, DoctorDto doctorDto)
        {
            var result = new List<SpecialtyDto>();

            if (doctor.DoctorSpecialties != null)
            {
                foreach (var specialty in doctor.DoctorSpecialties)
                {
                    result.Add(new SpecialtyDto() { Id = specialty.SpecialtyId, 
                    SpecialtyName = specialty.Specialties.SpecialtyName });
                }
            }
            return result;
        }

        private List<HospitalDto> MapForHospitals2(Doctor doctor, DoctorDto doctorDto)
        {
            var result = new List<HospitalDto>();

            if (doctor.DoctorHospitals != null)
            {
                foreach (var hospital in doctor.DoctorHospitals)
                {
                    result.Add(new HospitalDto() { Id = hospital.HospitalId, 
                    HospitalName = hospital.Hospital.HospitalName });
                }
            }
            return result;
        }

        private List<OfficeDto> MapForOffices(Doctor doctor, DoctorDto doctorDto)
        {
            var result = new List<OfficeDto>();

            if (doctor.Offices != null)
            {
                foreach (var office in doctor.Offices)
                {
                    result.Add(new OfficeDto() { Id = office.Id, 
                    Street = office.Street });
                }
            }
            return result;
        }




    }
}