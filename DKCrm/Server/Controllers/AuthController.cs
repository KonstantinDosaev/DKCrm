using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models.UserAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var message = await _authService.LoginAsync(request);
            return  message == "Ok" ? Ok(message) : BadRequest(message);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest parameters)
        {
            var message = await _authService.RegisterAsync(parameters);
            return message == "Ok" ? Ok(message) : BadRequest(message);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }

        [HttpGet]
        public CurrentUser CurrentUserInfo()
        {
            return _authService.CurrentUserInfo(User);
        }
    }
}
