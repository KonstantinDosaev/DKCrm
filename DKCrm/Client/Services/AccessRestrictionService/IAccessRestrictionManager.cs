using DKCrm.Shared.Models;
using System.Net.Http;

namespace DKCrm.Client.Services.AccessRestrictionService
{
    public interface IAccessRestrictionManager
    {
        Task<bool> CheckExistAccessAndContainsUserInArrayToComponentAsync(Guid componentId);
        Task<AccessRestriction> GetAccessFromComponentAsync(Guid componentId);
        Task<int> EditAccessToComponentAsync(AccessRestriction restriction);
        Task<int> RemoveAccessAsync(Guid accessId);
    }
}
