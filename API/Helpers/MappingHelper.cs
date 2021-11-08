using AutoMapper;
using Core.Entities;
using Core.Dtos;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace API.Helpers
{
    public class MappingHelper : Profile
    {
       
        public MappingHelper()
        {
            CreateMap<RegisterDto, ApplicationUser>().ReverseMap();

            CreateMap<DoctorCreateDto, ApplicationUser>().ReverseMap();
            CreateMap<DoctorCreateDto, Doctor>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.DoctorSpecialties, options => options.MapFrom(MapDoctorSpecialties));

          
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



    }
}