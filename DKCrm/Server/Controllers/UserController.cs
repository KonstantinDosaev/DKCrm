using DKCrm.Server.Data;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserDbContext _context;
        public UserController(UserManager<ApplicationUser> userManager, UserDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var allUsers = await _context.Users.ToListAsync();
            return Ok(allUsers);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users.Include(i=>i.Address).Where(user => user.Id == userId).FirstOrDefaultAsync();
            return Ok(user);
        }

        [HttpPost("update")]
        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            var userToBeUpdated = await _userManager.FindByNameAsync(user.UserName);
            if (userToBeUpdated == null)
                return IdentityResult.Failed(new IdentityError() { Description = $"User {user.UserName} was not found." });

            userToBeUpdated.FirstName = user.FirstName;
            userToBeUpdated.LastName = user.LastName;
            userToBeUpdated.Address = user.Address;
            userToBeUpdated.AdditionalAddress = user.AdditionalAddress;
            userToBeUpdated.PhoneNumber = user.PhoneNumber;
            userToBeUpdated.Email = user.Email;

            var result = await _userManager.UpdateAsync(userToBeUpdated);
            return result;
        }
        //[HttpPost("add")]
        //public async Task<IdentityResult> AddUserAsync(ApplicationUser user)
        //{
        //    var userToBeUpdated = await _userManager.FindByNameAsync(user.UserName);
        //    if (userToBeUpdated != null)
        //        return IdentityResult.Failed(new IdentityError() { Description = $"{user.UserName} уже существует." });
            
        //    var result = await _userManager.CreateAsync(user);
        //    return result;
        //}    
        [HttpPost("removerange")]
        public async Task RemoveUserRangeAsync(IEnumerable<ApplicationUser> users)
        {
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
        }


        [HttpGet("role")]
        public async Task<IActionResult> GetRole(string name)
        {
            var result = await _roleManager.FindByNameAsync(name);
            return Ok(result);
        }

        [HttpGet("roleAll")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return Ok(result);
        }
        [HttpGet("rolefromuser/{userId}")]
        public async Task<IActionResult> GetRoleFromUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.GetRolesAsync(user);
          return Ok(result);
        }
        [HttpPost("addtorole")]
        public async Task<IdentityResult> AddToRole(RoleRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return IdentityResult.Failed(new IdentityError() { Description = $"User {request.UserName} was not found." });

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            return result;
        }
        [HttpPost("updateuserrole")]
        public async Task<IdentityResult> UpdateUserRole(RoleRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return IdentityResult.Failed(new IdentityError() { Description = $"User {request.UserName} was not found." });
            var oldRole = await _userManager.GetRolesAsync(user);
           await _userManager.RemoveFromRolesAsync(user,oldRole);
           var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            return result;
        }
        //[HttpPost("addtoroles")]
        //public async Task<IdentityResult> AddToRoles(RolesRequest request)
        //{
        //    var user = await _userManager.FindByNameAsync(request.UserName);
        //    if (user == null)
        //        return IdentityResult.Failed(new IdentityError() { Description = $"User {request.UserName} was not found." });

        //    var result = await _userManager.AddToRolesAsync(user, request.RoleNames);
        //    return result;
        //}
        //[HttpPost("addrole/{roleName}")]
        //public async Task<IdentityResult> AddRole(string roleName)
        //{
        //    var role = new IdentityRole(){Name = roleName, NormalizedName = roleName.ToUpper()};
        //    var result = await _roleManager.CreateAsync(role);
        //    return result;
        //}
    }
}
