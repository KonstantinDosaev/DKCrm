using System.Security.Claims;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.UserAuth;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;


namespace DKCrm.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return "User does not exist";
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!singInResult.Succeeded) return "Invalid password";
            await _signInManager.SignInAsync(user, request.RememberMe);
            return "Ok";
        }

        public async Task<string> RegisterAsync(RegisterRequest parameters)
        {
            var user = new ApplicationUser();
            user.UserName = parameters.UserName;
            var result = await _userManager.CreateAsync(user, parameters.Password);
            if (!result.Succeeded) return result.Errors.FirstOrDefault()?.Description!;
            await _userManager.AddToRoleAsync(user, RoleNames.User);
            //return await Login(new LoginRequest
            //{
            //    UserName = parameters.UserName,
            //    Password = parameters.Password
            //});
            return "Ok";
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public CurrentUser CurrentUserInfo(ClaimsPrincipal user)
        { 
            return new CurrentUser
            {
                IsAuthenticated = user.Identity!.IsAuthenticated,
                UserName = user.Identity.Name!,
                Claims = user.Claims
                    .ToDictionary(c => c.Type, c => c.Value)
            };
        }
    }
}
