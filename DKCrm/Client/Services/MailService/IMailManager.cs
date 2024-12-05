using DKCrm.Shared.Requests;

namespace DKCrm.Client.Services.MailService
{
    public interface IMailManager
    {
        Task<bool> SendEmail(SendEmailRequest request);
    }
}
