using System.Text;
using System.Threading.Tasks;
using API.ErrorHandling;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
       
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService, 
            IMapper mapper, IDoctorRepository doctorRepository, IPatientRepository patientRepository,
            IAdminRepository adminRepository, IEmailService emailService, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _adminRepository = adminRepository;
            _emailService = emailService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            if (await CheckEmailExistsAsync(registerDto.Email)) return BadRequest("This email is taken");

            var user = _mapper.Map<ApplicationUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();
            user.PhoneNumber = registerDto.PhoneNumber;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ServerResponse(400));

            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = 
                    $"{_config["ApiAppUrl"]}/api/account/confirmemail?email={user.Email}&token={validEmailToken}";

            await _emailService.SendEmailAsync(user.Email, 
                "Confirm your email", $"<h1>Welcome to EDoctor</h1>" +
                $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");


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

            if (await CheckEmailExistsAsync(doctorCreateDto.Email)) return BadRequest("This email is taken");

            var user = _mapper.Map<ApplicationUser>(doctorCreateDto);
            
            user.UserName = doctorCreateDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, doctorCreateDto.Password);
            
            if (!result.Succeeded) return BadRequest(result.Errors);

            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = 
                    $"{_config["ApiAppUrl"]}/api/account/confirmemail?email={user.Email}&token={validEmailToken}";

            await _emailService.SendEmailAsync(user.Email, 
                "Confirm your email", $"<h1>Welcome to EDoctor</h1>" +
                $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");

            var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            var doctor = _mapper.Map<Doctor>(doctorCreateDto);

            await _doctorRepository.CreateDoctor(user.Id, doctor, user.LastName, user.FirstName);

           return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {          
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return BadRequest(new ServerResponse(400));

            if (!await _userManager.IsEmailConfirmedAsync(user)) 
                return Unauthorized(new ServerResponse(401, "Email is not confirmed"));

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

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                return NotFound(new ServerResponse(404));

            await _emailService.ConfirmEmailAsync(email, token);

            return Redirect($"{_config["AngularAppUrl"]}/account/email-confirmation");
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {         
            if (string.IsNullOrEmpty(dto.Email)) return NotFound(new ServerResponse(404));

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) return BadRequest(new ServerResponse(400));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_config["AngularAppUrl"]}/account/reset-password?email={dto.Email}&token={validToken}";

            await _emailService.SendEmailAsync(dto.Email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password <a href='{url}'>Click here</a></p>");   

            return Ok();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) return NotFound(new ServerResponse(404));

            var decodedToken = WebEncoders.Base64UrlDecode(dto.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, dto.Password);

            if (result.Succeeded) return Ok();

            return BadRequest(new ServerResponse(400));
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









