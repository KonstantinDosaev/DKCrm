using DKCrm.Shared.Models.UserAuth;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequest request);
        Task<string> RegisterAsync(RegisterRequest parameters);
        Task LogoutAsync();
        CurrentUser CurrentUserInfo(ClaimsPrincipal user);
    }
}
