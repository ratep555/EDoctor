using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;
       
        public AccountsController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService, 
            IMapper mapper, IDoctorRepository doctorRepository, IPatientRepository patientRepository,
            IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<ApplicationUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();
            user.PhoneNumber = registerDto.PhoneNumber;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest();

            var roleResult = await _userManager.AddToRoleAsync(user, "Patient");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            await _patientRepository.CreatePatient(user.Id, user.LastName, user.FirstName, registerDto.DateOfBirth);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                RoleName = await _userRepository.GetRoleName(user.Id),
                UserId = user.Id            };
        }

        [HttpPost("createdoctor")]
        public async Task<ActionResult<UserDto>> RegisterDoctor2(DoctorCreateDto doctorCreateDto)
        {
            if (await UserExists(doctorCreateDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<ApplicationUser>(doctorCreateDto);
            
            user.UserName = doctorCreateDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, doctorCreateDto.Password);
            
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            var doctor = _mapper.Map<Doctor>(doctorCreateDto);

            await _doctorRepository.CreateDoctor(user.Id, doctor, user.LastName, user.FirstName);

           return NoContent();
        }

    

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        
   

        
    }
}









