using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _userService.GetAsync());
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            return Ok(await _userService.GetAsync(userId));
        }

        [HttpPost("update")]
        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            return await _userService.PutAsync(user);
        }
        
        [HttpPost("removerange")]
        public async Task RemoveUserRangeAsync(IEnumerable<ApplicationUser> users)
        {
            await _userService.RemoveUserRangeAsync(users);
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetRole(string name)
        {
            return Ok(await _userService.GetRoleAsync(name));
        }

        [HttpGet("roleAll")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _userService.GetAllRolesAsync());
        }
        [HttpGet("rolefromuser/{userId}")]
        public async Task<IActionResult> GetRoleFromUser(string userId)
        {
            return Ok(await _userService.GetRoleFromUserAsync(userId));
        }
        [HttpPost("addtorole")]
        public async Task<IdentityResult> AddToRole(RoleRequest request)
        {
            return await _userService.AddToRoleAsync(request);
        }
        [HttpPost("updateuserrole")]
        public async Task<IdentityResult> UpdateUserRole(RoleRequest request)
        {
            return await _userService.UpdateUserRoleAsync(request);
        }
    }
}
