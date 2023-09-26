using DKCrm.Shared.Models.UserAuth;

namespace DKCrm.Client.Services.Auth
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        Task Register(RegisterRequest registerRequest);
        Task Logout();
        Task<CurrentUser> CurrentUserInfo();
    }
}
