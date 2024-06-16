using DKCrm.Client.Constants;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.DocumentService
{
    public class DocumentManager: IDocumentManager
    {
        private readonly HttpClient _httpClient;

        public DocumentManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        public async Task<string> CreateDoc()
        {
            var ytr = await _httpClient.GetFromJsonAsync<string>($"api/Document/GetDoc") ?? throw new InvalidOperationException();
            return ytr;
        }
        public async Task<bool> CreatePaymentInvoicePdfAsync(Guid orderId)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Document/CreatePaymentInvoicePdf/{orderId}", orderId, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
    }
}
