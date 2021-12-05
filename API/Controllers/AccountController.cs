using System.Threading.Tasks;
using API.ErrorHandling;
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
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IAdminRepository _adminRepository;
       
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService, 
            IMapper mapper, IDoctorRepository doctorRepository, IPatientRepository patientRepository,
            IAdminRepository adminRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _adminRepository = adminRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            if (await CheckEmailExistsAsync(registerDto.Email)) 
            return BadRequest("Please provide valid email");

            var user = _mapper.Map<ApplicationUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();
            user.PhoneNumber = registerDto.PhoneNumber;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ServerResponse(400));

            var roleResult = await _userManager.AddToRoleAsync(user, "Patient");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            await _patientRepository.CreatePatient(user, registerDto);
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                RoleName = await _adminRepository.GetRoleName(user.Id),
                UserId = user.Id            
            };
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("registerdoctor")]
        public async Task<ActionResult<UserDto>> RegisterDoctor(DoctorCreateDto doctorCreateDto)
        {
            if (await UserExists(doctorCreateDto.Username)) return BadRequest("Username is taken");

            if (await CheckEmailExistsAsync(doctorCreateDto.Email)) 
            return BadRequest("Please provide valid email");

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

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {          
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return BadRequest("Invalid request");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ServerResponse(401));

            if (user.LockoutEnd != null) return Unauthorized(new ServerResponse(401));

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                RoleName = await _adminRepository.GetRoleName(user.Id),
                UserId = user.Id
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }     

        private async Task<bool> CheckEmailExistsAsync(string email)
        {             
             return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}









