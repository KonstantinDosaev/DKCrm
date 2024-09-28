using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserDbContext _context;
        public UserService(UserManager<ApplicationUser> userManager, UserDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAsync()
        {
            var allUsers = await _context.Users.ToListAsync();
            return allUsers;
        }

        public async Task<ApplicationUser> GetAsync(string userId)
        {
            var user = await _context.Users.Include(i => i.Address).Where(user => user.Id == userId).FirstOrDefaultAsync();
            return user ?? throw new InvalidOperationException();
        }

        public async Task<IdentityResult> PutAsync(ApplicationUser user)
        {
            var userToBeUpdated = await _userManager.FindByNameAsync(user.UserName!);
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

        public async Task RemoveUserRangeAsync(IEnumerable<ApplicationUser> users)
        {
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
        }

        public async Task<IdentityRole> GetRoleAsync(string name)
        {
            var result = await _roleManager.FindByNameAsync(name);
            return result ?? throw new InvalidOperationException();
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<string>> GetRoleFromUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GetRolesAsync(user ?? throw new InvalidOperationException()); 
        }

        public async Task<IdentityResult> AddToRoleAsync(RoleRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName!);
            if (user == null)
                return IdentityResult.Failed(new IdentityError() { Description = $"User {request.UserName} was not found." });

            var result = await _userManager.AddToRoleAsync(user, request.RoleName!);
            return result;
        }

        public async Task<IdentityResult> UpdateUserRoleAsync(RoleRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName!);
            if (user == null)
                return IdentityResult.Failed(new IdentityError() { Description = $"User {request.UserName} was not found." });
            var oldRole = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRole);
            var result = await _userManager.AddToRoleAsync(user, request.RoleName!);
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
