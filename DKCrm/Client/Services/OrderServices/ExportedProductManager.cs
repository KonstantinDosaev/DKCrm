using DKCrm.Shared.Models.OrderModels;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.OrderServices
{
    public class ExportedProductManager : IExportedProductManager
    {
        private readonly HttpClient _httpClient;

        public ExportedProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ExportedProduct>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ExportedProduct>>("api/ExportedProduct/Get") ?? throw new InvalidOperationException();
        }

        public async Task<List<ExportedProduct>> GetNotEquippedAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ExportedProduct>>("api/ExportedProduct/GetNotEquipped") ?? throw new InvalidOperationException();
        }

        public async Task<ExportedProduct> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ExportedProduct>($"api/ExportedProduct/Get/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(ExportedProduct item)
        {
            var result = await _httpClient.PutAsJsonAsync("api/ExportedProduct/Put", item);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(ExportedProduct item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ExportedProduct/Post", item);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<ExportedProduct> items)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ExportedProduct/DeleteRange/removerange", items);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/ExportedProduct/Delete/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
