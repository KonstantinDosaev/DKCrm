using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace DKCrm.Server.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAsync();
        Task<ApplicationUser> GetAsync(string userId);
        Task<IdentityResult> PutAsync(ApplicationUser user);
        Task RemoveUserRangeAsync(IEnumerable<ApplicationUser> users);
        Task<IdentityRole> GetRoleAsync(string name);
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IEnumerable<string>> GetRoleFromUserAsync(string userId);
        Task<IdentityResult> AddToRoleAsync(RoleRequest request);
        Task<IdentityResult> UpdateUserRoleAsync(RoleRequest request);
    }
}
