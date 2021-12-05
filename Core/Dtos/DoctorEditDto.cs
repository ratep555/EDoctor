using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Dtos
{
    public class DoctorEditDto
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }


        [Required, MinLength(5), MaxLength(80)]
        public string Name { get; set; }


        [MaxLength(5000)]
        public string Resume { get; set; }

        public IFormFile Picture { get; set; }
        
               
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> SpecialtiesIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> HospitalsIds { get; set; }    
    }
}








