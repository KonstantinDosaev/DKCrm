using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests;
using static DKCrm.Client.Services.FnsRequesting.FnsModels;

namespace DKCrm.Server.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public  bool SendEmailAsync(SendEmailRequest request)
        {
            bool result;
            try
            {
                var credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);

                var mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName),
                    Subject = request.Subject,
                    Body = request.Message,
                    IsBodyHtml = true
                };
                if (request.Attachments != null)
                {
                    foreach (var file in request.Attachments)
                    {
                        mail.Attachments.Add(new Attachment(new MemoryStream(file.Value), $"{file.Key}"));
                    }
                }

                mail.To.Add(new MailAddress(request.Email));

                var client = new SmtpClient()
                {
                    Port = _emailSettings.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _emailSettings.MailServer,
                    EnableSsl = true,
                    Credentials = credentials,
                };
               

                client.Send(mail);
               result = true;
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }

            return result;
        }
    }
}
