using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces
{
    public interface IAccessRestrictionService
    {
        Task<bool> CheckExistAccessAndContainsUserInArrayToComponentAsync(Guid componentId, ClaimsPrincipal user);
        Task<AccessRestriction> GetAccessFromComponentAsync(Guid componentId);
        Task<int> EditAccessToComponentAsync(AccessRestriction restriction, ClaimsPrincipal user);
        Task<int> RemoveAccessAsync(Guid accessId);
    }
}
