using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Dtos
{
    public class OfficeCreateEditDto
    {
        public int Id { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<int?>))]
        public int? HospitalId { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal InitialExaminationFee { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal FollowUpExaminationFee { get; set; }

        
        [Required, MinLength(3), MaxLength(60)]
        public string Country { get; set; }


        [Required, MinLength(2), MaxLength(40)]
        public string City { get; set; }


        [Required, MinLength(2), MaxLength(40)]
        public string Street { get; set; }


        [Required, MinLength(100), MaxLength(2000)]
        public string Description { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }

        public IFormFile Picture { get; set; }


    }
}






