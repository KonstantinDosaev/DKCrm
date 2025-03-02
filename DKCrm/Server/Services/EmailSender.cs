using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Shared;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.util;
using MudBlazor;

namespace DKCrm.Server.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IUserService _userService;
        private readonly UserDbContext _contextUser;
        private readonly string _urlStatScheduler = $"http://localhost:5175/EmailSender/Start";
        private readonly string _urlStopScheduler = $"http://localhost:5175/EmailSender/Stop";
        private readonly EmailTaskService _sendEmailTask;
 
        public EmailSender(IOptions<EmailSettings> emailSettings, IUserService userService, UserDbContext contextUser 
          , EmailTaskService hostedService )
        {
            _userService = userService;
            _contextUser = contextUser;
            _sendEmailTask = hostedService;

            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(SendEmailRequest request, ClaimsPrincipal claims)
        {
            bool result;
            try
            {
                if ((bool)request.SendByUser)
                {
                     var currentUserId = claims.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .Select(a => a.Value).FirstOrDefault();
                     if (currentUserId!= null)
                     {
                         var userEmailSettings = await _userService.GetUserEmailSettingsByUserIdAsync(currentUserId, claims);
                         if (!string.IsNullOrEmpty(userEmailSettings.Password))
                         {
                             _emailSettings.SenderName = userEmailSettings.SenderName;
                             _emailSettings.Sender = userEmailSettings.Sender;
                             _emailSettings.Password = userEmailSettings.Password;
                             _emailSettings.MailServer = userEmailSettings.MailServer;
                             _emailSettings.MailPort = Convert.ToInt32(userEmailSettings.MailPort);
                         }
                     }
                }
                var credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                if (_emailSettings.Sender != null)
                {
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

                    var mailAddresses = request.Emails.Select(s => new MailAddress(s)).ToArray();
                    mail.To.AddRange(mailAddresses);
                    if (_emailSettings.MailServer != null)
                    {
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
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            return result;
        }

        public async Task<SortPagedResponse<SendEmailTask>> GetTasksBySortPagedSearchChapterAsync(SortPagedRequest<FilterTaskSendEmailTuple> request)
        {
            var data = _contextUser.SendEmailTasks.Select(s => new SendEmailTask()
            { 
                Id = s.Id,
                Emails = s.Emails,
                PhoneNumbers = s.PhoneNumbers,
                Subject = s.Subject,
                Message = s.Message,
                AttachmentOne = s.AttachmentOne,
                AttachmentTwo = s.AttachmentTwo,
                SendByUser = s.SendByUser,
                PrivetTask = s.PrivetTask,
                DateTimeInit = s.DateTimeInit,
                DateTimeCreate = s.DateTimeCreate,
                UseCreator = s.UseCreator, 
                UseCreatorId = s.UseCreatorId, 
            }).Select(s => s);

            /*if (!string.IsNullOrEmpty(request.GlobalFilterValue))
            {
                switch (request.GlobalFilterType)
                {
                    case GlobalFilterTypes.Product:
                        data = data.Where(w =>
                            w.PartNumber != null && w.PartNumber.ToLower().Contains(request.GlobalFilterValue.ToLower()));
                        break;
                    case GlobalFilterTypes.ExportedOrder:
                        {
                            var searchedListId = await _context.ExportedProducts
                                .Where(w => w.ExportedOrder != null && w.ExportedOrder.Number!.Contains(request.GlobalFilterValue))
                                .Select(s => s.ProductId).ToListAsync();
                            data = data.Where(w => searchedListId.Contains(w.Id));
                            break;
                        }
                    case GlobalFilterTypes.ImportedOrder:
                        {
                            var searchedListId = await _context.ImportedProducts
                                .Where(w => w.ImportedOrder != null && w.ImportedOrder.Number!.Contains(request.GlobalFilterValue))
                                .Select(s => s.ProductId).ToListAsync();
                            data = data.Where(w => searchedListId.Contains(w.Id));
                            break;
                        }
                    case GlobalFilterTypes.Company:
                        {
                            var searchedExportedListId = await _context.ExportedProducts
                                .Where(w => w.ExportedOrder != null && w.ExportedOrder.CompanyBuyer!.Name!.Contains(request.GlobalFilterValue))
                                .Select(s => s.ProductId).ToListAsync();
                            var searchedImportedListId = await _context.ImportedProducts
                                .Where(w => w.ImportedOrder != null && w.ImportedOrder.SellersCompany!.Name!.Contains(request.GlobalFilterValue))
                                .Select(s => s.ProductId).ToListAsync();
                            data = data.Where(w => searchedExportedListId.Contains(w.Id)
                                                   || searchedImportedListId.Contains(w.Id));
                            break;
                        }
                }
            }*/
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                request.SearchString = request.SearchString.Trim().ToLower();
                data = data.Where(w =>
                    w.Subject.ToLower().Contains(request.SearchString));
            }

            /*if (request.FilterTuple != null)
            {
                if (request.FilterTuple.CategoryId != null && request.FilterTuple.CategoryId != Guid.Empty && request.Chapter != ProductFromChapterNames.Category)
                {
                    data = data.Where(o => o.CategoryId == request.FilterTuple.CategoryId);
                }
                if (request.FilterTuple.BrandIdList != null && request.FilterTuple.BrandIdList.Any() && request.Chapter != ProductFromChapterNames.Brand)
                {
                    data = data.Where(o => request.FilterTuple.BrandIdList.Contains((Guid)o.BrandId!));
                }
                if (request.FilterTuple.ProductOptions != null && request.FilterTuple.ProductOptions.Any())
                {
                    var productsId = _context.ProductOptions
                        .Where(w => request.FilterTuple.ProductOptions.Contains(w.Id))
                        .Select(o => o.ProductId).Distinct();
                    data = data.Where(w => productsId.Contains(w.Id));
                }
            }*/

            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "subj_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Subject);
                    break;
                case "dateInit_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeInit);
                    break;
                case "dateCre_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreate);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return new SortPagedResponse<SendEmailTask>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };

        }


        public async Task<bool> AddOrUpdateSendEmailTask(SendEmailTask taskSend, ClaimsPrincipal user)
        {
            //|| string.IsNullOrEmpty(taskSend.UseCreatorId)
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId != null && (taskSend.Id == Guid.Empty)) taskSend.UseCreatorId = userId;
            if (taskSend.Emails.Trim().Length < 4)
                return false;
            var firstTask = _contextUser.SendEmailTasks.AsNoTracking().OrderBy(o => o.DateTimeInit).FirstOrDefault();
            if ((firstTask != null && firstTask.DateTimeInit > taskSend.DateTimeInit) || firstTask == null)
            {
                /*using (HttpClient client = new HttpClient())
                {
                    var t = await client.PostAsJsonAsync(_urlStatScheduler, 
                        new DateTimeRequest(){DateTimeInit = taskSend.DateTimeInit});
                }*/
                await _sendEmailTask.StopAsync(new CancellationToken(true));
                _sendEmailTask.DateTimeInit = taskSend.DateTimeInit;
               await _sendEmailTask.StartAsync(new CancellationToken(false));
            }

            _contextUser.SendEmailTasks.Entry(taskSend).State = taskSend.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;
            await _contextUser.SaveChangesAsync();
            return taskSend.Id != Guid.Empty;
        }
        public async Task<Guid> RemoveSendEmailTask(SendEmailTask taskSend)
        {
            var taskSendTwoFirst = _contextUser.SendEmailTasks.AsNoTracking().OrderBy(o => o.DateTimeInit).Take(2).ToArray();
            if (taskSendTwoFirst.Length > 1)
            {
                if (taskSendTwoFirst[0].DateTimeInit == taskSend.DateTimeInit)
                {
                    /*using (HttpClient client = new HttpClient())
                    {
                        var t = await client.PostAsJsonAsync(_urlStatScheduler, new DateTimeRequest() { DateTimeInit = taskSendTwoFirst[1].DateTimeInit } );
                    }*/
                    await _sendEmailTask.StopAsync(new CancellationToken(true));
                    _sendEmailTask.DateTimeInit = taskSendTwoFirst[1].DateTimeInit;
                    await _sendEmailTask.StartAsync(new CancellationToken(false));
                }
            }
            else
            {
                /*using (HttpClient client = new HttpClient())
                {
                    var t = await client.PostAsJsonAsync(_urlStopScheduler, new DateTimeRequest() { DateTimeInit = taskSend.DateTimeInit });
                }*/
                await _sendEmailTask.StopAsync(new CancellationToken(true));
            }

            _contextUser.SendEmailTasks.Entry(taskSend).State = EntityState.Deleted;
            await _contextUser.SaveChangesAsync();
            return taskSend.Id;
        }
        public async Task SendEmailTaskScheduler()
        {
            var taskSend = _contextUser.SendEmailTasks.AsNoTracking().OrderBy(o => o.DateTimeInit).Take(2).ToArray();
            if (taskSend.Any())
            {
                var attachments = new Dictionary<string, byte[]>();
                var attachmentOne = taskSend[0].AttachmentOne;
                if (attachmentOne != null) attachments.Add("1", attachmentOne);
                var attachmentTwo = taskSend[0].AttachmentOne;
                if (attachmentTwo != null) attachments.Add("1", attachmentTwo);
                var emails = taskSend[0].Emails.Split(";").ToList();


                await SendEmailAsync(new SendEmailRequest()
                    {
                        Message = taskSend[0].Message,
                        Attachments = attachments,
                        Emails = emails,
                        Subject = taskSend[0].Subject
                    }
                    , new ClaimsPrincipal());


                _contextUser.SendEmailTasks.Entry(taskSend[0]).State = EntityState.Deleted;
               
                if (taskSend.Length>1)
                {
                    /*using (HttpClient client = new HttpClient())
                    {
                        var t = await client.PostAsJsonAsync(_urlStatScheduler, new DateTimeRequest() { DateTimeInit = taskSend[1].DateTimeInit });
                    }*/
                    await _sendEmailTask.StopAsync(new CancellationToken(true));
                    _sendEmailTask.DateTimeInit = taskSend[1].DateTimeInit;
                    await _sendEmailTask.StartAsync(new CancellationToken(false));
                }
                else
                {
                    /*using (HttpClient client = new HttpClient())
                    {
                        var t = await client.PostAsJsonAsync(_urlStopScheduler, new DateTimeRequest() { DateTimeInit = taskSend[0].DateTimeInit });
                    }*/
                    await _sendEmailTask.StopAsync(new CancellationToken(true));
                }

                await RemoveSendEmailTask(taskSend[0]);
            }

        }
    }

}
public class DateTimeRequest
{
    public DateTime DateTimeInit { get; set; }
}