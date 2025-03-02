using System.Security.Claims;
using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.UserAuth;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DKCrm.Server.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserDbContext _context;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<ApplicationUser> userManager, UserDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
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
        public async Task<UserEmailSettings> GetUserEmailSettingsByUserIdAsync(string userId, ClaimsPrincipal claims)
        {
            /*var currentUserId = claims.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .Select(a => a.Value).FirstOrDefault();
            if (currentUserId == null)
                return new UserEmailSettings();
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user is { EmailConfirmed: false }) throw new Exception("Подтверждение по электронной почте не пройдено");*/

            var settings = await _context.UserEmailSettings.FirstOrDefaultAsync(f => f.UserId == userId);
            return settings ?? new UserEmailSettings();
        }
        public async Task<int> AddOrUpdateUserEmailSettingsAsync(UserEmailSettings settings)
        {
            var user = await _userManager.FindByIdAsync(settings.UserId);
            if (user == null)
                return 0;
            _context.UserEmailSettings.Entry(settings).State = settings.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync();
        }    
        public int CheckPass(string pass)
        {
                var mPass = _configuration[$"mpass"];
                if (mPass == null) return 0;
                return Base64Decode(pass) == mPass ? 1 : 0;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
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
