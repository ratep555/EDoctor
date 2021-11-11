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
            .ForMember(d => d.City, o => o.MapFrom(s => s.Office.City))
            .ForMember(d => d.Patient, o => o.MapFrom(s => s.Patient.Name))
            .ForMember(d => d.Doctor, o => o.MapFrom(s => s.Office.Doctor.Name))
            .ForMember(d => d.DoctorId, o => o.MapFrom(s => s.Office.DoctorId));

            CreateMap<AppointmentCreateEditDto, Appointment>().ReverseMap();

            CreateMap<RegisterDto, ApplicationUser>().ReverseMap();

            CreateMap<DoctorCreateDto, ApplicationUser>().ReverseMap();

            CreateMap<DoctorCreateDto, Doctor>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.DoctorSpecialties, options => options.MapFrom(MapDoctorSpecialties))
                .ForMember(x => x.DoctorHospitals, options => options.MapFrom(MapDoctorHospitals));
            
            CreateMap<Hospital, HospitalDto>().ReverseMap();

            CreateMap<Office, OfficeDto>()
                .ForMember(d => d.Doctor, o => o.MapFrom(s => s.Doctor.Name))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Location.Y))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Location.X));

            CreateMap<OfficeCreateEditDto, Office>()
                .ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

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



    }
}