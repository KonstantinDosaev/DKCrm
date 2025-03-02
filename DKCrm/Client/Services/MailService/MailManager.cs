using DKCrm.Client.Constants;
using DKCrm.Shared.Models.OrderModels;
using System.Net.Http.Json;
using DKCrm.Shared.Requests;
using DKCrm.Shared;
using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Models;

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
        public async Task<SortPagedResponse<SendEmailTask>> GetTasksBySortAsync(SortPagedRequest<FilterTaskSendEmailTuple> request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Mails/GetTasksBySortPagedSearch", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<SortPagedResponse<SendEmailTask>>() ?? throw new InvalidOperationException();

        }
        public async Task<bool> AddOrUpdateSendEmailTask(SendEmailTask taskSend)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Mails/AddOrUpdateSendEmailTask", taskSend, JsonOptions.JsonIgnore);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveTask(SendEmailTask taskSend)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Mails/RemoveSendEmailTask", taskSend);
            return result.IsSuccessStatusCode;
        }
    }
}
