using DKCrm.Shared.Models.OrderModels;
using System.Net.Http.Json;
using DKCrm.Client.Constants;
using DKCrm.Shared.Requests;

namespace DKCrm.Client.Services.OrderServices
{
    public class ImportedProductManager : IImportedProductManager
    {
        private readonly HttpClient _httpClient;

        public ImportedProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ImportedProduct>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ImportedProduct>>("api/ImportedProduct/Get") ?? throw new InvalidOperationException();
        }

        public async Task<ImportedProduct> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ImportedProduct>($"api/ImportedProduct/Get/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<List<ImportedProduct>> GetNotEquippedAsync(Guid productId)
        {
            return await _httpClient.GetFromJsonAsync<List<ImportedProduct>>($"api/ImportedProduct/GetNotEquipped/{productId}") ?? throw new InvalidOperationException();
        }

        public async Task<List<ImportedProduct>> GetAllNotAddToOrderAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ImportedProduct>>("api/ImportedProduct/GetAllNotAddToOrder") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(ImportedProduct item)
        {
            var result = await _httpClient.PutAsJsonAsync("api/ImportedProduct/Put", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(ImportedProduct item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportedProduct/Post", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> MergeImportedProductsAsync(MergeImportedProductsRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportedProduct/MergeImportedProducts", request, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<ImportedProduct> items)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportedProduct/DeleteRange/removerange", items);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/ImportedProduct/Delete/{id}");
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSourcesOrderItems(ImportedProduct item)
        {
            var result = await _httpClient.PostAsJsonAsync("api/ImportedProduct/UpdateSourcesOrderItems", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
    }
}
