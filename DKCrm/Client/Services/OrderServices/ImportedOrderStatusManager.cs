using DKCrm.Client.Constants;
using DKCrm.Shared.Models.OrderModels;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.OrderServices
{
    public class ImportedOrderStatusManager : IImportedOrderStatusManager
    {
        private readonly HttpClient _httpClient;

        public ImportedOrderStatusManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ImportedOrderStatus>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ImportedOrderStatus>>("api/ImportedOrderStatus/Get") ?? throw new InvalidOperationException();
        }

        public async Task<ImportedOrderStatus> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ImportedOrderStatus>($"api/ImportedOrderStatus/Get/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(ImportedOrderStatus item)
        {
            var result = await _httpClient.PutAsJsonAsync("api/ImportedOrderStatus/Put", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(ImportedOrderStatus item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportedOrderStatus/Post", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<ImportedOrderStatus> items)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportedOrderStatus/DeleteRange/removerange", items);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/ImportedOrderStatus/Delete/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
