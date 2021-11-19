using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RatingsController : BaseApiController
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;
        public RatingsController(IRatingRepository ratingRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ratingRepository = ratingRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRate([FromBody] RatingDto ratingDto)
        {
            var userId = User.GetUserId();

            if (await _ratingRepository.CheckIfThisIsDoctorsPatient(ratingDto.DoctorId, userId))
            {
                return BadRequest("You have not visited this doctor yet!");
            }

            var currentRate = await _ratingRepository.FindCurrentRate(ratingDto.DoctorId, userId);

            if (currentRate == null)
            {
                await _ratingRepository.AddRating(ratingDto, userId);
            }
            else
            {
                currentRate.Rate = ratingDto.Rating;
                await _ratingRepository.Save();
            }

            return NoContent();
        }
    }
}
