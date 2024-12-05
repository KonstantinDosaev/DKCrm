using DKCrm.Client.Constants;
using DKCrm.Shared.Models.OrderModels;
using System.Net.Http.Json;
using DKCrm.Shared.Requests;

namespace DKCrm.Client.Services.MailService
{
    public class MailManager : IMailManager
    {
        private readonly HttpClient _httpClient;

        public MailManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> SendEmail(SendEmailRequest request)
        {
            var result = await _httpClient
                .PostAsJsonAsync($"api/Mails/SendEmail", request, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
    }
}
