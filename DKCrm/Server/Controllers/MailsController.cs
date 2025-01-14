using DKCrm.Server.Interfaces;
using DKCrm.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MailsController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public MailsController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        [HttpPost]
        public IActionResult SendEmail(SendEmailRequest request)
        {
            return Ok( _emailSender.SendEmailAsync(request));
        }
    }
}
