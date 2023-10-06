using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace DKCrm.Client.Services.UserService
{
    public interface IUserManagerCustom
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser> GetUserDetailsAsync(string userId);
        Task UpdateUser(ApplicationUser user);
        Task AddUser(ApplicationUser user);
        Task RemoveUsers(IEnumerable<ApplicationUser> user);
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleAsync(string roleName);
        Task<List<string>> GetRoleFromUser(string userId);
        Task AddUserToRole(RoleRequest request);
        Task UpdateUserRole(RoleRequest request);
        //Task AddUserToRoles(RolesRequest request);
        //Task AddRole(string roleName);

    }
}
