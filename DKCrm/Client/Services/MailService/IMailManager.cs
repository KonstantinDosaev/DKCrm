using DKCrm.Shared;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests;

namespace DKCrm.Client.Services.MailService
{
    public interface IMailManager
    {
        Task<bool> SendEmail(SendEmailRequest request);
        Task<bool> AddOrUpdateSendEmailTask(SendEmailTask taskSend);
        Task<SortPagedResponse<SendEmailTask>> GetTasksBySortAsync(SortPagedRequest<FilterTaskSendEmailTuple> request);
        Task<bool> RemoveTask(SendEmailTask task);
    }
}
