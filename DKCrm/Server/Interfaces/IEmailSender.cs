using System.Security.Claims;
using DKCrm.Shared;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests;

namespace DKCrm.Server.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(SendEmailRequest request, ClaimsPrincipal user);

        Task<SortPagedResponse<SendEmailTask>> GetTasksBySortPagedSearchChapterAsync(
            SortPagedRequest<FilterTaskSendEmailTuple> request);
        Task SendEmailTaskScheduler();
        Task<bool> AddOrUpdateSendEmailTask(SendEmailTask taskSend, ClaimsPrincipal user);
        Task<Guid> RemoveSendEmailTask(SendEmailTask taskSend);
    }
}
