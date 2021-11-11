using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class EDoctorContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, 
        IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public EDoctorContext(DbContextOptions<EDoctorContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<ApplicationRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(s => s.Patient)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(s => s.Office)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);   

            modelBuilder.Entity<Rating>()
                .HasOne(s => s.Patient)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);   

            modelBuilder.Entity<DoctorSpecialty>()
                .HasKey(x => new { x.DoctorId, x.SpecialtyId }); 

            modelBuilder.Entity<DoctorHospital>()
                .HasKey(x => new { x.DoctorId, x.HospitalId }); 
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
        public DbSet<DoctorHospital> DoctorHospitals { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Rating> Ratings { get; set; }


    }
}









