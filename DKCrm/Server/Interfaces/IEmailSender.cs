using DKCrm.Shared.Requests;

namespace DKCrm.Server.Interfaces
{
    public interface IEmailSender
    {
        bool SendEmailAsync(SendEmailRequest request);
    }
}
