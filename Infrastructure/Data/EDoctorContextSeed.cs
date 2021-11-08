using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class EDoctorContextSeed
    {
        
    public static async Task SeedUserAsync(
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager)
        {
            var roles = new List<ApplicationRole>
            {
                new ApplicationRole{Name = "Admin"},
                new ApplicationRole{Name = "Doctor"},
                new ApplicationRole{Name = "Patient"},

            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            if (await userManager.Users.AnyAsync()) return;
            
            var admin = new ApplicationUser
            {
                FirstName = "Bob",
                LastName = "Bobbity",
                Email = "bob@test.com",
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");               
            await userManager.AddToRolesAsync(admin, new[] {"Admin"});                                        
        }

        public static async Task SeedEntitiesAsync(EDoctorContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Hospitals.Any())
                {
                    var hospitalsData = File.ReadAllText("../Infrastructure/Data/SeedData/hospitals.json");
                    var hospitals = JsonSerializer.Deserialize<List<Hospital>>(hospitalsData);

                    foreach (var item in hospitals)
                    {
                        context.Hospitals.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.Specialties.Any())
                {
                    var specialtiesData = File.ReadAllText("../Infrastructure/Data/SeedData/specialties.json");
                    var specialties = JsonSerializer.Deserialize<List<Specialty>>(specialtiesData);

                    foreach (var item in specialties)
                    {
                        context.Specialties.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 
            
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<EDoctorContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
