using System.Linq;
using System.Threading.Tasks;
using API.ErrorHandling;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminRepository _adminRepository;
        public AdminController(UserManager<ApplicationUser> userManager,
            IAdminRepository adminRepository)
        {
            _userManager = userManager;
            _adminRepository = adminRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<UserToReturnDto>>> GetAllUsers(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _adminRepository.GetCountForUsers();
            var list = await _adminRepository.GetAllUsers(queryParameters);

            return Ok(new Pagination<UserToReturnDto>
            (queryParameters.Page, queryParameters.PageCount, count, list));
        }


       // [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)

                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);
              
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockUser(int id)
        {
            var user = await _adminRepository.FindUserById(id);

            if (user == null)
            {
                return NotFound(new ServerResponse(404));
            }

           await _adminRepository.UnlockUser(id);

           return NoContent();
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockUser(int id)
        {
            var user = await _adminRepository.FindUserById(id);

            if (user == null)
            {
                return NotFound(new ServerResponse(404));
            }

            if (user.Email == "bob@test.com")
            {
                return BadRequest("You cannot lock this user!");
            }

           await _adminRepository.LockUser(id);

           return NoContent();
        }

    }
}










