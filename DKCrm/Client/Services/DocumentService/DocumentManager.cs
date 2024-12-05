using DKCrm.Client.Constants;
using System.Net.Http.Json;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.DocumentService
{
    public class DocumentManager: IDocumentManager
    {
        private readonly HttpClient _httpClient;

        public DocumentManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        public async Task<IEnumerable<InfoSetFromDocumentToOrder>> GetAllInfoSetsForOrderAsync(Guid orderId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<InfoSetFromDocumentToOrder>>
                ($"api/Document/GetAllDocumentFileInfoSetsForOrder/{orderId}") ?? throw new InvalidOperationException();
        }
        public async Task<IEnumerable<InfoSetFromDocumentToOrder>> GetOneInfoSetFromDocumentFileAsync(Guid infoSetId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<InfoSetFromDocumentToOrder>>
                ($"api/Document/GetInfoSetFromDocumentFile/{infoSetId}") ?? throw new InvalidOperationException();
        }
        public async Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId)
        {
            return await _httpClient.GetFromJsonAsync<byte[]>
                ($"api/Document/GetDocumentBytArray/{infoSetId}") ?? throw new InvalidOperationException();
        }
        public async Task<bool> RemoveDocumentAsync(Guid infoSetId)
        {
            var result = await _httpClient.DeleteAsync($"api/Document/RemoveDocument/{infoSetId}");
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> CreatePaymentInvoicePdfAsync(CreatePaymentInvoiceRequest request)
        {
            var result = await _httpClient
                .PostAsJsonAsync($"api/Document/CreatePaymentInvoicePdf/{request}", request, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> CreateOrderSpecificationPdfAsync(CreateOrderSpecificationRequest request)
        {
            var result = await _httpClient
                .PostAsJsonAsync($"api/Document/CreateOrderSpecificationPdf/{request}", request, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<byte[]> AddStampToPdfAsync(AddStampToPdfRequest request)
        {
            var result = await _httpClient
                .PostAsJsonAsync($"api/Document/SetStampAndGetDocumentBytArray/{request}", request, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<byte[]>() ?? throw new InvalidOperationException();
        }
    }
}
