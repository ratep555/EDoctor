using System.Linq;
using API.Helpers;
using API.Services;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace API.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();
            services.AddScoped<IHospitalRepository, HospitalRepository>();
            services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
    
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileStorageService, InAppStorageService>();
            services.AddHttpContextAccessor();

            services.AddDbContext<EDoctorContext>(options =>
               options.UseSqlServer(
                   config.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.UseNetTopologySuite()));
            
            services.AddAutoMapper(typeof(MappingHelper).Assembly);

            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
                var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                config.AddProfile(new MappingHelper(geometryFactory));
            }).CreateMapper());

            services.AddSingleton<GeometryFactory>(NtsGeometryServices
               .Instance.CreateGeometryFactory(srid: 4326));


            return services;
        }
    }
}