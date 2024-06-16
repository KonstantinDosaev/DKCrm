using DKCrm.Client.Constants;
using DKCrm.Shared.Models;
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

        public async Task<List<ExportedProduct>> GetNotEquippedAsync(Guid productId)
        {
            return await _httpClient.GetFromJsonAsync<List<ExportedProduct>>($"api/ExportedProduct/GetNotEquipped/{productId}") ?? throw new InvalidOperationException();
        }

        public async Task<List<ExportedProduct>> GetAllNotEquippedAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ExportedProduct>>($"api/ExportedProduct/GetAllNotEquipped") ?? throw new InvalidOperationException();
        }
        public async Task<ExportedProduct> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ExportedProduct>($"api/ExportedProduct/Get/{id}") ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<ExportedProduct>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterExportedProductTuple> request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/ExportedProduct/GetBySortPagedSearchChapter", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<SortPagedResponse<ExportedProduct>>() ?? throw new InvalidOperationException();

        }

        public async Task<bool> UpdateAsync(ExportedProduct item)
        {
            var result = await _httpClient.PutAsJsonAsync("api/ExportedProduct/Put", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(ExportedProduct item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ExportedProduct/Post", item, JsonOptions.JsonIgnore);
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

        public async Task<bool> UpdateSourcesOrderItems(ExportedProduct item)
        {
            var result = await _httpClient.PostAsJsonAsync("api/ExportedProduct/UpdateSourcesOrderItems", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
    }
}
