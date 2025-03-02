using DKCrm.Server.Interfaces;
using DKCrm.Shared;
using DKCrm.Shared.Models;
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
            return Ok( _emailSender.SendEmailAsync(request, User));
        }
        [HttpPost]
        public async Task<SortPagedResponse<SendEmailTask>> GetTasksBySortPagedSearch(
            SortPagedRequest<FilterTaskSendEmailTuple> request)
        {
            return await _emailSender.GetTasksBySortPagedSearchChapterAsync(request);
        }

        [HttpGet]
        public async Task SendEmailTaskScheduler()
        {
            await _emailSender.SendEmailTaskScheduler();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateSendEmailTask(SendEmailTask taskSend)
            => Ok(await _emailSender.AddOrUpdateSendEmailTask(taskSend, User));
        [HttpPost]
        public async Task<IActionResult> RemoveSendEmailTask(SendEmailTask taskSend)
            => Ok(await _emailSender.RemoveSendEmailTask(taskSend));

    }
}
